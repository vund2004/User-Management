﻿using Data.Models.AccountModels.Response;
using Data.Models.CouponModels;
using FastFoodWeb_Client.HttpRepositories.Interfaces;
using Newtonsoft.Json;
using System.Text;

namespace FastFoodWeb_Client.HttpRepositories.Services
{
    public class HttpCouponService : IHttpCouponService
    {
        private readonly HttpClient _httpClient;

        public HttpCouponService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CouponForView>> GetAllCoupon()
        {
            var response = await _httpClient.GetAsync("https://localhost:44346/api/CouponsApi/get-coupons"); // Sử dụng đường dẫn tương đối
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var coupons = JsonConvert.DeserializeObject<List<CouponForView>>(content)!;
                return coupons;
            }
            throw new ApplicationException(content);
        }

        public async Task<CouponForView> GetCoupons(Guid id)
        {
            var response = await _httpClient.GetAsync($"https://localhost:44346/api/CouponsApi/get-coupon/{id}"); // Sử dụng đường dẫn tương đối
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var coupon = JsonConvert.DeserializeObject<CouponForView>(content)!;
                return coupon;
            }
            throw new ApplicationException("Coupon not found.");
        }

        public async Task UpdateCoupons(Guid id, CouponForUpdate model)
        {
            string data = JsonConvert.SerializeObject(model);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            var putResult = await _httpClient.PutAsync("https://localhost:44346/api/CouponsApi/update-coupon?id=" + id, content); // Sử dụng đường dẫn tương đối
            var putContent = await putResult.Content.ReadAsStringAsync();

            if (!putResult.IsSuccessStatusCode)
            {
                var errorResponse = JsonConvert.DeserializeObject<BaseResponseMessage>(putContent);
                throw new ApplicationException(errorResponse?.Errors ?? "Đã xảy ra lỗi không xác định.");
            }
        }

        public async Task DeleteCoupons(Guid id)
        {
            var deleteResult = await _httpClient.DeleteAsync($"https://localhost:44346/api/CouponsApi/delete-coupon/{id}"); // Sử dụng đường dẫn tương đối
            var deleteContent = await deleteResult.Content.ReadAsStringAsync();

            if (!deleteResult.IsSuccessStatusCode)
            {
                throw new ApplicationException(deleteContent);
            }
        }

        public async Task CreateCoupons(CouponForCreate model) // Thay đổi thành async
        {
            string data = JsonConvert.SerializeObject(model);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            var postResult = await _httpClient.PostAsync("https://localhost:44346/api/CouponsApi/create-coupon", content); // Sử dụng đường dẫn tương đối
            var postContent = await postResult.Content.ReadAsStringAsync();

            if (!postResult.IsSuccessStatusCode)
            {
                var errorResponse = JsonConvert.DeserializeObject<BaseResponseMessage>(postContent);
                throw new ApplicationException(errorResponse?.Errors ?? "Đã xảy ra lỗi không xác định.");
            }
        }
    }
}
