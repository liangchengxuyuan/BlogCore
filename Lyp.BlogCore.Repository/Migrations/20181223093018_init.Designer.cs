﻿// <auto-generated />
using System;
using Lyp.BlogCore.Repository.MySqlEFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Lyp.BlogCore.Repository.Migrations
{
    [DbContext(typeof(MySqlDbContext))]
    [Migration("20181223093018_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024");

            modelBuilder.Entity("Lyp.BlogCore.Models.Models.Admin", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("PassWord");

                    b.Property<string>("UserName");

                    b.HasKey("ID");

                    b.ToTable("admins");
                });

            modelBuilder.Entity("Lyp.BlogCore.Models.Models.BlogArticle", b =>
                {
                    b.Property<int>("bID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("bAuthor");

                    b.Property<int>("bCommentNum");

                    b.Property<string>("bContent");

                    b.Property<DateTime>("bCreateTime");

                    b.Property<int>("bReadNum");

                    b.Property<string>("bTitle");

                    b.Property<int>("cID");

                    b.HasKey("bID");

                    b.ToTable("blogArticles");
                });

            modelBuilder.Entity("Lyp.BlogCore.Models.Models.Category", b =>
                {
                    b.Property<int>("cID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("cName");

                    b.HasKey("cID");

                    b.ToTable("categories");
                });

            modelBuilder.Entity("Lyp.BlogCore.Models.Models.Comment", b =>
                {
                    b.Property<int>("cmID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("bID");

                    b.Property<string>("cmCommentBody");

                    b.Property<string>("cmCommentator");

                    b.Property<DateTime>("cmCreateTime");

                    b.HasKey("cmID");

                    b.ToTable("comments");
                });

            modelBuilder.Entity("Lyp.BlogCore.Models.Models.GuestBook", b =>
                {
                    b.Property<int>("gID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("gBody");

                    b.Property<DateTime>("gCreateTime");

                    b.Property<string>("gUserName");

                    b.HasKey("gID");

                    b.ToTable("guestBooks");
                });

            modelBuilder.Entity("Lyp.BlogCore.Models.Models.UserInfo", b =>
                {
                    b.Property<int>("uID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("uCreateTime");

                    b.Property<short>("uIsAdministrators")
                        .HasColumnType("bit");

                    b.Property<string>("uPassWord");

                    b.Property<string>("uUserName");

                    b.HasKey("uID");

                    b.ToTable("userInfos");
                });
#pragma warning restore 612, 618
        }
    }
}
