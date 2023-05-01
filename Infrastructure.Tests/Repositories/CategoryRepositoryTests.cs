using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Repositories
{
    public sealed class CategoryRepositoryTests
    {
        [Fact]
        public void Categories_WhenCalled_ReturnCategories()
        {
            Category[] categories =
            {
                new()
                {
                    CategoryId = 1,
                    Name = "First",
                },
                new()
                {
                    CategoryId = 2,
                    Name = "Second",
                }
            };

            var options = new DbContextOptionsBuilder<ApiDataContext>()
                .UseInMemoryDatabase(databaseName: "Categories")
                .Options;

            using (var context = new ApiDataContext(options))
            {
                context.Categories.AddRange(categories);
                context.SaveChanges();
            }

            using (var context = new ApiDataContext(options))
            {
                CategoryRepository categoryRepository = new(context);
                var result = (categoryRepository.Categories.AsNoTracking()).ToArray();

                result.Should().BeOfType<Category[]>();
                result.Should().BeEquivalentTo(categories);
            }
        }

        [Fact]
        public async void CreateCategoryAsync_WhenCalled_AddCategory()
        {
            Category[] categories =
            {
                new()
                {
                    CategoryId = 1,
                    Name = "First",
                },
                new()
                {
                    CategoryId = 2,
                    Name = "Second",
                }
            };
            Category category = new()
            {
                CategoryId = 0,
                Name = "Test",
            };

            var options = new DbContextOptionsBuilder<ApiDataContext>()
                .UseInMemoryDatabase(databaseName: "CreateCategory")
                .Options;

            using (var context = new ApiDataContext(options))
            {
                context.Categories.AddRange(categories);
                context.SaveChanges();
            }

            using (var context = new ApiDataContext(options))
            {
                CategoryRepository categoryRepository = new(context);
                var oldResult = (categoryRepository.Categories.AsNoTracking()).ToArray();
                await categoryRepository.CreateCategoryAsync(category);
                var newResult = (categoryRepository.Categories.AsNoTracking()).ToArray();

                oldResult.Should().BeOfType<Category[]>();
                oldResult.Should().BeEquivalentTo(categories);
                newResult.Should().BeOfType<Category[]>();
                newResult.Should().NotBeNullOrEmpty();
                newResult.Should().HaveCount(categories.Length + 1);
            }
        }

        [Fact]
        public async void UpdateCategoryAsync_WhenCalled_UpdateCategory()
        {
            Category category = new()
            {
                CategoryId = 2,
                Name = "Second",
            };
            Category[] categories =
            {
                new()
                {
                    CategoryId = 1,
                    Name = "First",
                },
                category,
            };



            var options = new DbContextOptionsBuilder<ApiDataContext>()
                .UseInMemoryDatabase(databaseName: "UpdateCategory")
                .Options;

            using (var context = new ApiDataContext(options))
            {
                context.Categories.AddRange(categories);
                context.SaveChanges();
            }

            using (var context = new ApiDataContext(options))
            {
                CategoryRepository categoryRepository = new(context);
                category.Name = "UpdatedName";

                await categoryRepository.UpdateCategoryAsync(category);
                var result = (categoryRepository.Categories.AsNoTracking()).ToArray();

                result.Should().BeOfType<Category[]>();
                result.Should().HaveCount(categories.Length);
                result[1].Should().BeEquivalentTo(category);
            }
        }

        [Fact]
        public async void DeleteCategoryAsync_WhenCalled_DeleteCategory()
        {
            Category category = new()
            {
                CategoryId = 2,
                Name = "Second",
            };
            Category[] categories =
            {
                new()
                {
                    CategoryId = 1,
                    Name = "First",
                },
                category,
            };

            var options = new DbContextOptionsBuilder<ApiDataContext>()
                .UseInMemoryDatabase(databaseName: "DeleteCategory")
                .Options;

            using (var context = new ApiDataContext(options))
            {
                context.Categories.AddRange(categories);
                context.SaveChanges();
            }

            using (var context = new ApiDataContext(options))
            {
                CategoryRepository categoryRepository = new(context);
                var oldResult = (categoryRepository.Categories.AsNoTracking()).ToArray();
                await categoryRepository.DeleteCategoryAsync(category);
                var newResult = (categoryRepository.Categories.AsNoTracking()).ToArray();

                oldResult.Should().BeOfType<Category[]>();
                oldResult.Should().BeEquivalentTo(categories);
                newResult.Should().BeOfType<Category[]>();
                newResult.Should().NotBeNullOrEmpty();
                newResult.Should().HaveCount(categories.Length - 1);
            }
        }
    }
}
