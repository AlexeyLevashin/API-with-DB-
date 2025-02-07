using FluentMigrator;

namespace FirstRestAPI.Migrations;

[Migration(1)]
public class M0000_InitialMigration:Migration
{
    public override void Up()
    {
        Create.Table("users")
            .WithColumn("id").AsInt64().Identity().PrimaryKey()
            .WithColumn("email").AsString().Unique().NotNullable()
            .WithColumn("password_hash").AsString().NotNullable()
            .WithColumn("role").AsString().NotNullable();
        
        Create.Table("posts")
            .WithColumn("id").AsInt64().Identity().PrimaryKey()
            .WithColumn("author_id").AsInt64().NotNullable().ForeignKey("FK_Posts_Users", "users", "id")
            .WithColumn("idempotency_key").AsString().NotNullable().Unique()
            .WithColumn("title").AsString().NotNullable()
            .WithColumn("content").AsString().NotNullable()
            .WithColumn("created_at").AsDateTime().NotNullable()
            .WithColumn("updated_at").AsDateTime().NotNullable()
            .WithColumn("is_published").AsString().NotNullable();
        
        Create.Table("images")
            .WithColumn("id").AsInt64().Identity().NotNullable().PrimaryKey()
            .WithColumn("post_id").AsInt64().NotNullable().ForeignKey("FK_Images_Posts", "posts", "id")
            .WithColumn("image_name").AsString().NotNullable()
            .WithColumn("created_at").AsDateTime().NotNullable();
    }

    public override void Down()
    {
        Delete.Table("users");
        Delete.Table("posts");
        Delete.Table("images");
    }
}