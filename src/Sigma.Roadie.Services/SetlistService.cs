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

            foreach(var item in list)
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
            dest.Description = update.Description;

            await entities.SaveChangesAsync();

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
            var rel = new SetlistScene()
            {
                SceneId = sceneId,
                SetlistId = setlistId
            };

            entities.SetlistScene.Add(rel);

            await entities.SaveChangesAsync();
        }


        public async Task RemoveSceneFromSetlist(Guid setlistId, Guid sceneId)
        {
            var rel = await (from p in entities.SetlistScene where p.SetlistId == setlistId && p.SceneId == sceneId select p).FirstOrDefaultAsync();

            entities.Remove(rel);

            await entities.SaveChangesAsync();
        }


    }

}
