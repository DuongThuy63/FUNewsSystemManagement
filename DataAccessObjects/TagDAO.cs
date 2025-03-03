using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;

namespace DataAccessObjects
{
    public class TagDAO
    {
        public static List<Tag> GetTag()
        {
            var listTags = new List<Tag>();
            try
            {
                using var context = new FunewsManagementContext();
                listTags = context.Tags.ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return listTags;
        }
        public static void AddTag(Tag tag)
        {

            using var context = new FunewsManagementContext();

            context.Tags.Add(tag);
            context.SaveChanges();
        }
    }
}
