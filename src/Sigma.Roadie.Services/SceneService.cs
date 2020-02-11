using Microsoft.EntityFrameworkCore;
using Sigma.Roadie.Domain.DataModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Sigma.Roadie.Services
{

    public class SceneService
    {

        RoadieEntities entities;

        public SceneService(RoadieEntities entities)
        {
            this.entities = entities;
        }


        public Task<List<Scene>> GetScenes()
        {
            return (from p in entities.Scene.Include(q => q.MediaFile) orderby p.Name select p).AsNoTracking().ToListAsync();
        }

        public async Task<Scene> GetSceneById(Guid sceneId)
        {
            var item = await (from p in entities.Scene.Include(q => q.MediaFile) where p.SceneId == sceneId select p).AsNoTracking().FirstOrDefaultAsync();

            if (item == null)
            {
                item = new Scene();
            }

            return item;
        }


        public async Task<SetlistScene> GetActiveScene()
        {
            var active = await (from p in entities.SetlistScene
                                    .Include(q => q.Scene)
                                    .ThenInclude(q => q.MediaFile)
                                where p.IsActive == true
                                select p).AsNoTracking().FirstOrDefaultAsync();

            return active;
        }


        public async Task<Scene> UpdateScene(Scene update)
        {
            var dest = await (from p in entities.Scene where p.SceneId == update.SceneId select p).FirstOrDefaultAsync();

            if (dest == null)
            {
                dest = new Scene()
                {
                    SceneId = Guid.NewGuid()
                };
                entities.Scene.Add(dest);
            }

            dest.Name = update.Name;
            dest.Description = update.Description ?? string.Empty;
            dest.Duration = update.Duration;

            await entities.SaveChangesAsync();

            return dest;
        }


        public async Task RemoveScene(Guid sceneId)
        {
            var dest = await (from p in entities.Scene where p.SceneId == sceneId select p).FirstOrDefaultAsync();

            var deps = await (from p in entities.MediaFile where p.SceneId == sceneId select p).ToListAsync();

            entities.RemoveRange(deps);
            entities.Remove(dest);

            await entities.SaveChangesAsync();
        }


        public async Task<MediaFile> UpdateMediaFileToScene(Guid sceneId, MediaFile update)
        {
            var dest = await (from p in entities.MediaFile where p.MediaFileId == update.MediaFileId select p).FirstOrDefaultAsync();

            if (dest == null)
            {
                dest = new MediaFile()
                {
                    MediaFileId = Guid.NewGuid(),
                    SceneId = sceneId
                };
                entities.MediaFile.Add(dest);
            }

            dest.Name = update.Name;
            dest.PlayAt = update.PlayAt;
            dest.Type = update.Type;
            dest.Url1 = update.Url1;
            dest.Url2 = update.Url2;

            await entities.SaveChangesAsync();

            return dest;
        }


        public async Task RemoveMediaFileFromScene(Guid sceneId, Guid mediaFileId)
        {
            var rel = await (from p in entities.MediaFile where p.MediaFileId == mediaFileId select p).FirstOrDefaultAsync();

            entities.Remove(rel);

            await entities.SaveChangesAsync();
        }


        public async Task<SetlistScene> PlayNextScene(Guid setlistId)
        {
            var active = await GetActiveScene();

            int nextIndex = (active?.Index ?? 0) + 1;

            var next = await (from p in entities.SetlistScene
                              where p.SetlistId == setlistId && p.Index == nextIndex
                              select p).AsNoTracking().FirstOrDefaultAsync();

            return await PlayScene(setlistId, next?.SceneId ?? Guid.Empty);
        }


        public async Task<SetlistScene> PlayScene(Guid setlistId, Guid sceneId)
        {
            var allscenes = await entities.SetlistScene.ToListAsync();
            allscenes.ForEach(q => q.IsActive = false);

            var next = await (from p in entities.SetlistScene where p.SetlistId == setlistId && p.SceneId == sceneId select p).FirstOrDefaultAsync();

            if (next != null)
            {
                next.IsActive = true;
            }

            await entities.SaveChangesAsync();

            return await GetActiveScene();
        }


    }

}