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
            dest.LocalUri = update.LocalUri;

            await entities.SaveChangesAsync();

            return dest;
        }


        public async Task RemoveMediaFileFromScene(Guid sceneId, Guid mediaFileId)
        {
            var rel = await (from p in entities.MediaFile where p.MediaFileId == mediaFileId select p).FirstOrDefaultAsync();

            entities.Remove(rel);

            await entities.SaveChangesAsync();
        }


        public async Task<MediaFile> GetMediaFileById(Guid mediaFileId)
        {
            var file = await (from p in entities.MediaFile
                                .Include(q => q.Scene)
                              where p.MediaFileId == mediaFileId
                              select p).FirstOrDefaultAsync();

            return file;
        }


    }

}