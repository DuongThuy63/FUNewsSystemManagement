using BusinessObjects;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repositories;

namespace Services
{
    public class TagService : ITagService
    {
        private readonly ITagRepository iTagRepository;

        public TagService()
        {
            iTagRepository = new TagRepository();
        }

        public List<Tag> GetTags()
        {
            return iTagRepository.GetTags();
        }

        public void AddTag(Tag tag)
        {
             iTagRepository.addTag(tag);
        }
    }
}
