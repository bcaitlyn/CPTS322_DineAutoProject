// This file was created by Yevin 4/24
using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;

namespace DineAuto.Testing
{
    public partial class Tests
    {
        private static readonly string restaurantReviewPath = "Pages/Testing/TestingTables/testRestaurantReviews.json";
        private static readonly string itemReviewPath = "Pages/Testing/TestingTables/testItemReviews.json";

        // Test 01: Customer leaves a restaurant review (Should pass)
        public static void TestLeaveRestaurantReview()
        {
            string city = "Pullman";
            string restaurant = "Test Bistro";
            string username = "customer1";

            var newReview = new
            {
                Username = username,
                Rating = 5,
                Comment = "Fantastic place!",
                OwnerReply = (string)null
            };

            var reviewsData = new Dictionary<string, Dictionary<string, List<object>>>
            {
                [city] = new Dictionary<string, List<object>>
                {
                    [restaurant] = new List<object> { newReview }
                }
            };

            string json = JsonSerializer.Serialize(reviewsData, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(restaurantReviewPath, json);

            string result = File.ReadAllText(restaurantReviewPath);
            bool passed = result.Contains("Fantastic place!") && result.Contains("customer1");

            Console.WriteLine(passed ? "Leave Restaurant Review Test PASSED." : "Leave Restaurant Review Test FAILED.");
        }

        // Test 02: Customer leaves an item review (Should pass)
        public static void TestLeaveItemReview()
        {
            string city = "Pullman";
            string restaurant = "Test Bistro";
            string item = "Lasagna";
            string username = "customer1";

            var itemReview = new
            {
                Username = username,
                Rating = 4,
                Comment = "Tasty and filling"
            };

            var reviewsData = new Dictionary<string, Dictionary<string, Dictionary<string, List<object>>>>
            {
                [city] = new Dictionary<string, Dictionary<string, List<object>>>
                {
                    [restaurant] = new Dictionary<string, List<object>>
                    {
                        [item] = new List<object> { itemReview }
                    }
                }
            };

            string json = JsonSerializer.Serialize(reviewsData, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(itemReviewPath, json);

            string result = File.ReadAllText(itemReviewPath);
            bool passed = result.Contains("Tasty and filling") && result.Contains("Lasagna");

            Console.WriteLine(passed ? "Leave Item Review Test PASSED." : "Leave Item Review Test FAILED.");
        }

        // Test 03: Owner replies to a restaurant review (Should pass)
        public static void TestOwnerReplyToReview()
        {
            string city = "Pullman";
            string restaurant = "Test Bistro";

            var existingReview = new Dictionary<string, Dictionary<string, List<Dictionary<string, object>>>>
            {
                [city] = new Dictionary<string, List<Dictionary<string, object>>>
                {
                    [restaurant] = new List<Dictionary<string, object>>
                    {
                        new Dictionary<string, object>
                        {
                            { "Username", "customer1" },
                            { "Rating", 5 },
                            { "Comment", "Excellent food!" },
                            { "OwnerReply", null }
                        }
                    }
                }
            };

            File.WriteAllText(restaurantReviewPath, JsonSerializer.Serialize(existingReview, new JsonSerializerOptions { WriteIndented = true }));

            // Load, add reply
            var json = File.ReadAllText(restaurantReviewPath);
            var reviewsData = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, List<Dictionary<string, object>>>>>(json);

            if (reviewsData != null &&
                reviewsData.ContainsKey(city) &&
                reviewsData[city].ContainsKey(restaurant) &&
                reviewsData[city][restaurant].Count > 0)
            {
                reviewsData[city][restaurant][0]["OwnerReply"] = "Thank you for the kind words!";
            }

            File.WriteAllText(restaurantReviewPath, JsonSerializer.Serialize(reviewsData, new JsonSerializerOptions { WriteIndented = true }));

            string result = File.ReadAllText(restaurantReviewPath);
            bool passed = result.Contains("Thank you for the kind words!");

            Console.WriteLine(passed ? "Owner Reply Test PASSED." : "Owner Reply Test FAILED.");
        }
    }
}