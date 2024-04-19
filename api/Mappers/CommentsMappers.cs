using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comment;
using api.Models;

namespace api.Mappers
{
    public static class CommentsMappers
    {
        public static CommentDto toCommentDto(this Comment commentModel)
        {
            return new CommentDto
            {
                Id = commentModel.Id,
                Title = commentModel.Title,
                Content = commentModel.Content,
                CreatedOn = commentModel.CreatedOn,
                Stock = commentModel.Stock.ToStockForComment()
            };
        }
        public static Comment toCommentFromCreate(this CreateCommentRequestDto commentDto)
        {
            return new Comment
            {
                Title = commentDto.Title,
                Content = commentDto.Content
            };
        }

        public static CommentForStockDto toCommentForStock(this Comment commentDto)
        {
            return new CommentForStockDto
            {
                Id = commentDto.Id,
                Title = commentDto.Title,
                Content = commentDto.Content,
                CreatedOn = commentDto.CreatedOn,
            };
        }
    }
}