using Microsoft.EntityFrameworkCore;
using Sigma.Roadie.Domain.DataModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Sigma.Roadie.Services
{

    public class SetlistService
    {

        RoadieEntities entities;

        public SetlistService(RoadieEntities entities)
        {
            this.entities = entities;
        }


        public async Task<List<Setlist>> GetSetlists()
        {
            var list = await (from p in entities.Setlist.Include(q => q.SetlistScene).ThenInclude(q => q.Scene) select p).ToListAsync();

            foreach (var item in list)
            {
                item.SetlistScene = item.SetlistScene.OrderBy(q => q.Index).ToList();
            }

            return list;
        }


        public async Task<Setlist> GetSetlistById(Guid setlistId)
        {
            var item = await (from p in entities.Setlist.Include(q => q.SetlistScene).ThenInclude(q => q.Scene) where p.SetlistId == setlistId select p).FirstOrDefaultAsync();

            if (item == null)
            {
                item = new Setlist();
            }

            item.SetlistScene = item.SetlistScene.OrderBy(q => q.Index).ToList();

            return item;
        }


        public async Task<Setlist> UpdateSetlist(Setlist update)
        {
            var dest = await (from p in entities.Setlist where p.SetlistId == update.SetlistId select p).FirstOrDefaultAsync();

            if (dest == null)
            {
                dest = new Setlist()
                {
                    CreatedAt = DateTime.Now,
                    IsActive = false,
                    SetlistId = Guid.NewGuid()
                };
                entities.Setlist.Add(dest);
            }

            dest.Name = update.Name;
            dest.Description = update.Description ?? string.Empty;

            await entities.SaveChangesAsync();

            return dest;
        }


        public async Task<Setlist> SetActiveSetlist(Guid setlistId)
        {
            var allsets = await entities.Setlist.ToListAsync();
            allsets.ForEach(q => q.IsActive = false);
            var allscenes = await entities.SetlistScene.ToListAsync();
            allscenes.ForEach(q => q.IsActive = false);

            var dest = await (from p in entities.Setlist where p.SetlistId == setlistId select p).FirstOrDefaultAsync();

            if (dest != null)
            {
                dest.IsActive = true;

                await entities.SaveChangesAsync();
            }

            return dest;
        }


        public async Task RemoveSetlist(Guid setlistId)
        {
            var dest = await (from p in entities.Setlist where p.SetlistId == setlistId select p).FirstOrDefaultAsync();

            var deps = await (from p in entities.SetlistScene where p.SetlistId == setlistId select p).ToListAsync();

            entities.RemoveRange(deps);
            entities.Remove(dest);

            await entities.SaveChangesAsync();
        }


        public async Task AddSceneToSetlist(Guid setlistId, Guid sceneId)
        {
            if (entities.SetlistScene.Any(q => q.SetlistId == setlistId && q.SceneId == sceneId)) return;


            int index = await (from p in entities.SetlistScene where p.SetlistId == setlistId select p).CountAsync() + 1;

            var rel = new SetlistScene()
            {
                SceneId = sceneId,
                SetlistId = setlistId,
                Index = Convert.ToInt16(index),
                IsActive = false
            };

            entities.SetlistScene.Add(rel);

            await entities.SaveChangesAsync();
        }


        public async Task RemoveSceneFromSetlist(Guid setlistId, Guid sceneId)
        {
            var rel = await (from p in entities.SetlistScene where p.SetlistId == setlistId && p.SceneId == sceneId select p).FirstOrDefaultAsync();

            if (rel != null)
            {
                entities.Remove(rel);

                await entities.SaveChangesAsync();

                await ReorderIndexes(setlistId);
            }
        }


        public async Task UpdateSceneIndexInSetlist(Guid setlistId, Guid sceneId, int change)
        {
            var scenes = await (from p in entities.SetlistScene where p.SetlistId == setlistId select p).ToListAsync();

            var i0 = scenes.FirstOrDefault(p => p.SceneId == sceneId);
            var i1 = scenes.FirstOrDefault(p => p.Index == i0.Index + change);

            var io = i0.Index;
            i0.Index = i1.Index;
            i1.Index = io;

            await entities.SaveChangesAsync();
        }


        async Task ReorderIndexes(Guid setlistId)
        {
            var scenes = await (from p in entities.SetlistScene where p.SetlistId == setlistId orderby p.Index select p).ToListAsync();

            for (var nindex = 0; nindex < scenes.Count; nindex++)
            {
                scenes[nindex].Index = Convert.ToInt16(nindex + 1);
            }

            await entities.SaveChangesAsync();
        }


    }

}
