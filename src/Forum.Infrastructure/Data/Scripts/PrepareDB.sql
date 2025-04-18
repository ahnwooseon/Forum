-- 1. Create logins for forum_admin and forum_app
CREATE LOGIN forum_admin WITH PASSWORD = 'Auietsrn0';
CREATE LOGIN forum_app WITH PASSWORD = 'Auietsrn0';
GO

-- 2. Create forum database
CREATE DATABASE forum;
GO

-- 3. Switch context to the forum database
USE forum;
GO

-- 4. Set forum_admin as the database owner
ALTER AUTHORIZATION ON DATABASE::forum TO forum_admin;
GO

-- 5. Create role for forum_app
CREATE ROLE forum_app_role;
GO

-- 6. Grant permissions to forum_app_role
-- Grant CRUD permissions (SELECT, INSERT, UPDATE, DELETE) on dbo schema to forum_app_role
GRANT SELECT, INSERT, UPDATE, DELETE ON SCHEMA::dbo TO forum_app_role;

-- Allow table creation for forum_app_user
GRANT CREATE TABLE TO forum_app_role;

-- Allow forum_app_user to modify forum database objects
GRANT ALTER ON DATABASE::forum TO forum_app_role;

-- Allow forum_app_user to establish relationships (REFERENCES) and modify tables in dbo schema
GRANT REFERENCES, ALTER ON SCHEMA::dbo TO forum_app_role;
GO

-- 7. Create a user for forum_app
CREATE USER forum_app_user FOR LOGIN forum_app;
GO

-- 8. Add forum_app_user to the role
EXEC sp_addrolemember 'forum_app_role', 'forum_app_user';
GO
