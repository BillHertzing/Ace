SELECT TagLists.ID, [dbo].[TagListToTags].TagList, [dbo].[TagListToTags].Tags,TagNames.ID, TagNames.TagName
FROM  TagLists, [dbo].[TagListToTags], TagNames
where TagLists.ID = [dbo].[TagListToTags].[TagList]
AND [dbo].[TagListToTags].Tags = TagNames.ID

       
