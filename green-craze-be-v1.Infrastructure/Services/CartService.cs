﻿using AutoMapper;
using green_craze_be_v1.Application.Common.Exceptions;
using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.Cart;
using green_craze_be_v1.Application.Model.Paging;
using green_craze_be_v1.Application.Specification.Cart;
using green_craze_be_v1.Domain.Entities;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Infrastructure.Services
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<long> AddVariantItemToCart(AddVariantItemToCartRequest request)
        {
            var cart = await _unitOfWork.Repository<Cart>().GetEntityWithSpec(new CartSpecification(request.UserId))
                ?? throw new NotFoundException("Cannot find cart of user");

            var cartItem = await _unitOfWork.Repository<CartItem>().GetEntityWithSpec(new CartItemSpecification(cart.Id, request.VariantId));

            var ci = new CartItem();
            if (cartItem == null)
            {
                var variant = await _unitOfWork.Repository<Variant>().GetById(request.VariantId)
                    ?? throw new NotFoundException("Cannot find product variant");
                ci.Quantity = request.Quantity;
                ci.Variant = variant;
                cart.CartItems.Add(ci);
                _unitOfWork.Repository<Cart>().Update(cart);
            }
            else
            {
                cartItem.Quantity += 1;
                _unitOfWork.Repository<CartItem>().Update(cartItem);
            }

            var isSuccess = await _unitOfWork.Save() > 0;

            if (!isSuccess) throw new Exception("Cannot add product to cart");

            return cartItem == null ? ci.Id : -1;
        }

        public async Task<bool> DeleteCartItem(long cartItemId, string userId)
        {
            var cart = await _unitOfWork.Repository<Cart>().GetEntityWithSpec(new CartSpecification(userId))
                ?? throw new NotFoundException("Cannot find cart of user");
            var cartItem = cart.CartItems.FirstOrDefault(x => x.Id == cartItemId)
                ?? throw new NotFoundException("Cannot find cart item");

            cart.CartItems.Remove(cartItem);
            _unitOfWork.Repository<Cart>().Update(cart);

            var isSuccess = await _unitOfWork.Save() > 0;

            if (!isSuccess) throw new Exception("Cannot remove product from cart");

            return true;
        }

        public async Task<PaginatedResult<CartItemDto>> GetCartByUser(GetCartPagingRequest request)
        {
            var cartItems = await _unitOfWork.Repository<CartItem>().ListAsync(new CartItemSpecification(request, isPaging: true));
            var count = await _unitOfWork.Repository<CartItem>().CountAsync(new CartItemSpecification(request));

            var cartDtos = new List<CartItemDto>();
            cartItems.ForEach(x =>
            {
                var isPromotion = x.Variant.PromotionalItemPrice != x.Variant.ItemPrice;
                cartDtos.Add(new CartItemDto()
                {
                    Id = x.Id,
                    CreatedAt = x.CreatedAt,
                    CreatedBy = x.CreatedBy,
                    UpdatedAt = x.UpdatedAt,
                    UpdatedBy = x.UpdatedBy,
                    Quantity = x.Quantity,
                    TotalPrice = x.Variant.Quantity * x.Variant.ItemPrice,
                    TotalPromotionalPrice = x.Variant.Quantity * x.Variant.PromotionalItemPrice,
                    Sku = x.Variant.Sku,
                    VariantName = x.Variant.Name,
                    VariantPrice = x.Variant.ItemPrice,
                    VariantPromotionalPrice = x.Variant.PromotionalItemPrice,
                    IsPromotion = isPromotion,
                    VariantQuantity = x.Variant.Quantity,
                });
            });
            return new PaginatedResult<CartItemDto>(cartDtos, request.PageIndex, count, request.PageSize);
        }

        public async Task<CartItemDto> GetCartItemById(long cartItemId, string userId)
        {
            var cart = await _unitOfWork.Repository<Cart>().GetEntityWithSpec(new CartSpecification(userId))
                ?? throw new NotFoundException("Cannot find cart of user");
            var cartItem = cart.CartItems.FirstOrDefault(x => x.Id == cartItemId)
                ?? throw new NotFoundException("Cannot find cart item");

            var isPromotion = cartItem.Variant.PromotionalItemPrice != cartItem.Variant.ItemPrice;
            return new CartItemDto()
            {
                Id = cartItem.Id,
                CreatedAt = cartItem.CreatedAt,
                CreatedBy = cartItem.CreatedBy,
                UpdatedAt = cartItem.UpdatedAt,
                UpdatedBy = cartItem.UpdatedBy,
                Quantity = cartItem.Quantity,
                TotalPrice = cartItem.Variant.Quantity * cartItem.Variant.ItemPrice,
                TotalPromotionalPrice = cartItem.Variant.Quantity * cartItem.Variant.PromotionalItemPrice,
                Sku = cartItem.Variant.Sku,
                VariantName = cartItem.Variant.Name,
                VariantPrice = cartItem.Variant.ItemPrice,
                VariantPromotionalPrice = cartItem.Variant.PromotionalItemPrice,
                IsPromotion = isPromotion,
                VariantQuantity = cartItem.Variant.Quantity,
            };
        }

        public async Task<bool> UpdateCartItemQuantity(UpdateCartItemQuantityRequest request)
        {
            if (request.Quantity <= 0)
                throw new ValidationException("Quantity must be positive number");
            var cart = await _unitOfWork.Repository<Cart>().GetEntityWithSpec(new CartSpecification(request.UserId))
                ?? throw new NotFoundException("Cannot find cart of user");

            var cartItem = cart.CartItems.FirstOrDefault(x => x.Id == request.CartItemId)
                ?? throw new NotFoundException("Cannot find cart item");

            cartItem.Quantity = request.Quantity;

            _unitOfWork.Repository<Cart>().Update(cart);

            var isSuccess = await _unitOfWork.Save() > 0;

            if (!isSuccess) throw new Exception("Cannot update product quantity");

            return true;
        }
    }
}