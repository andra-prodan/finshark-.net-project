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
                CreatedBy = commentModel.AppUser.UserName,
                StockId = commentModel.StockId,
            };
        }
        public static Comment toCommentFromCreate(this CreateCommentRequestDto commentDto, int stockId, string appUserId)
        {
            return new Comment
            {
                Title = commentDto.Title,
                Content = commentDto.Content,
                StockId = stockId,
                AppUserId = appUserId
            };
        }

        public static Comment toCommentFromUpdate(this UpdateCommentRequestDto commentRequestDto)
        {
            return new Comment
            {
                Title = commentRequestDto.Title,
                Content = commentRequestDto.Content
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
                CreatedBy = commentDto.AppUser.UserName
            };
        }
    }
}