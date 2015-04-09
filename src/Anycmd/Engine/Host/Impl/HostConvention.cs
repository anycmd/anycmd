
namespace Anycmd.Engine.Host.Impl
{
    using Ac;
    using Ac.Infra;
    using Exceptions;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Web;
    using System.Web.Hosting;
    using Util;

    public class HostConvention : IAppConfig
    {
        public HostConvention()
        {
            var dir = string.Empty;
            if (HostingEnvironment.IsHosted)
            {
                dir = HttpRuntime.BinDirectory;
            }
            else
            {
                dir = AppDomain.CurrentDomain.BaseDirectory;
            }
            #region plugin
            this.PluginBaseDirectory = (pluginType) =>
            {
                var buildInPluginsBaseDirectory = Path.Combine(dir, "Plugins");
                switch (pluginType)
                {
                    case PluginType.Plugin:
                        return buildInPluginsBaseDirectory;
                    case PluginType.MessageProvider:
                        return Path.Combine(buildInPluginsBaseDirectory, "MessageProviders");
                    case PluginType.EntityProvider:
                        return Path.Combine(buildInPluginsBaseDirectory, "EntityProviders");
                    case PluginType.InfoStringConverter:
                        return Path.Combine(buildInPluginsBaseDirectory, "InfoStringConverters");
                    case PluginType.InfoConstraint:
                        return Path.Combine(buildInPluginsBaseDirectory, "InfoConstraints");
                    case PluginType.MessageTransfer:
                        return Path.Combine(buildInPluginsBaseDirectory, "MessageTransfers");
                    default:
                        throw new AnycmdException("意外的插件类型");
                };
            };
            this.Plugins = new List<IPlugin>
            {
            };
            #endregion
            this.CurrentAcSessionCacheKey = "_acSession";
            this.EnableClientCache = false;
            this.EnableOperationLog = true;
            this.SelfAppSystemCode = "Anycmd";
            this.SqlServerTableColumnsSelect =
            #region SqlServerTableColumnsSelect
 @"SELECT  [UnionAll1].[Id] AS [Id] ,
        [UnionAll1].[Ordinal] AS [Ordinal] ,
        [Extent1].[CatalogName] AS [CatalogName] ,
        [Extent1].[SchemaName] AS [SchemaName] ,
        [Extent1].[Name] AS [TableName] ,
        [UnionAll1].[Name] AS [Name] ,
        [UnionAll1].[IsNullable] AS [IsNullable] ,
        [UnionAll1].[TypeName] AS [TypeName] ,
        [UnionAll1].[MaxLength] AS [MaxLength] ,
        [UnionAll1].[Precision] AS [Precision] ,
        [UnionAll1].[DateTimePrecision] AS [DateTimePrecision] ,
        [UnionAll1].[Scale] AS [Scale] ,
        [UnionAll1].[IsIdentity] AS [IsIdentity] ,
        [UnionAll1].[IsStoreGenerated] AS [IsStoreGenerated] ,
        [UnionAll1].[Default] AS DefaultValue ,
        [UnionAll1].[Description] AS [Description] ,
        CASE WHEN ( [Project5].[C2] IS NULL ) THEN CAST(0 AS BIT)
             ELSE [Project5].[C2]
        END AS [IsPrimaryKey]
FROM    ( SELECT    QUOTENAME(TABLE_SCHEMA) + QUOTENAME(TABLE_NAME) [Id] ,
                    TABLE_CATALOG [CatalogName] ,
                    TABLE_SCHEMA [SchemaName] ,
                    TABLE_NAME [Name]
          FROM      INFORMATION_SCHEMA.TABLES
          WHERE     TABLE_TYPE = 'BASE TABLE'
        ) AS [Extent1]
        INNER JOIN ( SELECT [Extent2].[Id] AS [Id] ,
                            [Extent2].[Name] AS [Name] ,
                            [Extent2].[Ordinal] AS [Ordinal] ,
                            [Extent2].[IsNullable] AS [IsNullable] ,
                            [Extent2].[TypeName] AS [TypeName] ,
                            [Extent2].[MaxLength] AS [MaxLength] ,
                            [Extent2].[Precision] AS [Precision] ,
                            [Extent2].[DateTimePrecision] AS [DateTimePrecision] ,
                            [Extent2].[Scale] AS [Scale] ,
                            [Extent2].[IsIdentity] AS [IsIdentity] ,
                            [Extent2].[IsStoreGenerated] AS [IsStoreGenerated] ,
                            [Extent2].[Default] ,
                            [Extent2].[Description] AS [Description] ,
                            0 AS [C1] ,
                            [Extent2].[ParentId] AS [ParentId]
                     FROM   ( SELECT    QUOTENAME(c.TABLE_SCHEMA)
                                        + QUOTENAME(c.TABLE_NAME)
                                        + QUOTENAME(c.COLUMN_NAME) [Id] ,
                                        QUOTENAME(c.TABLE_SCHEMA)
                                        + QUOTENAME(c.TABLE_NAME) [ParentId] ,
                                        c.COLUMN_NAME [Name] ,
                                        ( SELECT    CAST(VALUE AS NVARCHAR(MAX))
                                          FROM      ::
                                                    FN_LISTEXTENDEDPROPERTY(N'MS_Description',
                                                              N'SCHEMA',
                                                              c.TABLE_SCHEMA,
                                                              N'TABLE',
                                                              c.TABLE_NAME,
                                                              N'COLUMN',
                                                              c.COLUMN_NAME)
                                        ) AS [Description] ,
                                        c.ORDINAL_POSITION [Ordinal] ,
                                        CAST(CASE c.IS_NULLABLE
                                               WHEN 'YES' THEN 1
                                               WHEN 'NO' THEN 0
                                               ELSE 0
                                             END AS BIT) [IsNullable] ,
                                        CASE WHEN c.DATA_TYPE IN ( 'varchar',
                                                              'nvarchar',
                                                              'varbinary' )
                                                  AND c.CHARACTER_MAXIMUM_LENGTH = -1
                                             THEN c.DATA_TYPE + '(max)'
                                             ELSE c.DATA_TYPE
                                        END AS [TypeName] ,
                                        c.CHARACTER_MAXIMUM_LENGTH [MaxLength] ,
                                        CAST(c.NUMERIC_PRECISION AS INTEGER) [Precision] ,
                                        CAST(c.DATETIME_PRECISION AS INTEGER) [DateTimePrecision] ,
                                        CAST(c.NUMERIC_SCALE AS INTEGER) [Scale] ,
                                        c.COLLATION_CATALOG [CollationCatalog] ,
                                        c.COLLATION_SCHEMA [CollationSchema] ,
                                        c.COLLATION_NAME [CollationName] ,
                                        c.CHARACTER_SET_CATALOG [CharacterSetCatalog] ,
                                        c.CHARACTER_SET_SCHEMA [CharacterSetSchema] ,
                                        c.CHARACTER_SET_NAME [CharacterSetName] ,
                                        CAST(0 AS BIT) AS [IsMultiSet] ,
                                        CAST(COLUMNPROPERTY(OBJECT_ID(QUOTENAME(c.TABLE_SCHEMA)
                                                              + '.'
                                                              + QUOTENAME(c.TABLE_NAME)),
                                                            c.COLUMN_NAME,
                                                            'IsIdentity') AS BIT) AS [IsIdentity] ,
                                        CAST(COLUMNPROPERTY(OBJECT_ID(QUOTENAME(c.TABLE_SCHEMA)
                                                              + '.'
                                                              + QUOTENAME(c.TABLE_NAME)),
                                                            c.COLUMN_NAME,
                                                            'IsComputed')
                                        | CASE WHEN c.DATA_TYPE = 'timestamp'
                                               THEN 1
                                               ELSE 0
                                          END AS BIT) AS [IsStoreGenerated] ,
                                        c.COLUMN_DEFAULT AS [Default]
                              FROM      INFORMATION_SCHEMA.COLUMNS c
                                        INNER JOIN INFORMATION_SCHEMA.TABLES t ON c.TABLE_CATALOG = t.TABLE_CATALOG
                                                              AND c.TABLE_SCHEMA = t.TABLE_SCHEMA
                                                              AND c.TABLE_NAME = t.TABLE_NAME
                                                              AND t.TABLE_TYPE = 'BASE TABLE'
                            ) AS [Extent2]
                     UNION ALL
                     SELECT [Extent3].[Id] AS [Id] ,
                            [Extent3].[Name] AS [Name] ,
                            [Extent3].[Ordinal] AS [Ordinal] ,
                            [Extent3].[IsNullable] AS [IsNullable] ,
                            [Extent3].[TypeName] AS [TypeName] ,
                            [Extent3].[MaxLength] AS [MaxLength] ,
                            [Extent3].[Precision] AS [Precision] ,
                            [Extent3].[DateTimePrecision] AS [DateTimePrecision] ,
                            [Extent3].[Scale] AS [Scale] ,
                            [Extent3].[IsIdentity] AS [IsIdentity] ,
                            [Extent3].[IsStoreGenerated] AS [IsStoreGenerated] ,
                            [Extent3].[Default] ,
                            [Extent3].[Description] AS [Description] ,
                            6 AS [C1] ,
                            [Extent3].[ParentId] AS [ParentId]
                     FROM   ( SELECT    QUOTENAME(c.TABLE_SCHEMA)
                                        + QUOTENAME(c.TABLE_NAME)
                                        + QUOTENAME(c.COLUMN_NAME) [Id] ,
                                        QUOTENAME(c.TABLE_SCHEMA)
                                        + QUOTENAME(c.TABLE_NAME) [ParentId] ,
                                        c.COLUMN_NAME [Name] ,
                                        ( SELECT    CAST(VALUE AS NVARCHAR(MAX))
                                          FROM      ::
                                                    FN_LISTEXTENDEDPROPERTY(N'MS_Description',
                                                              N'SCHEMA',
                                                              c.TABLE_SCHEMA,
                                                              N'TABLE',
                                                              c.TABLE_NAME,
                                                              N'COLUMN',
                                                              c.COLUMN_NAME)
                                        ) AS [Description] ,
                                        c.ORDINAL_POSITION [Ordinal] ,
                                        CAST(CASE c.IS_NULLABLE
                                               WHEN 'YES' THEN 1
                                               WHEN 'NO' THEN 0
                                               ELSE 0
                                             END AS BIT) [IsNullable] ,
                                        CASE WHEN c.DATA_TYPE IN ( 'varchar',
                                                              'nvarchar',
                                                              'varbinary' )
                                                  AND c.CHARACTER_MAXIMUM_LENGTH = -1
                                             THEN c.DATA_TYPE + '(max)'
                                             ELSE c.DATA_TYPE
                                        END AS [TypeName] ,
                                        c.CHARACTER_MAXIMUM_LENGTH [MaxLength] ,
                                        CAST(c.NUMERIC_PRECISION AS INTEGER) [Precision] ,
                                        CAST(c.DATETIME_PRECISION AS INTEGER) AS [DateTimePrecision] ,
                                        CAST(c.NUMERIC_SCALE AS INTEGER) [Scale] ,
                                        c.COLLATION_CATALOG [CollationCatalog] ,
                                        c.COLLATION_SCHEMA [CollationSchema] ,
                                        c.COLLATION_NAME [CollationName] ,
                                        c.CHARACTER_SET_CATALOG [CharacterSetCatalog] ,
                                        c.CHARACTER_SET_SCHEMA [CharacterSetSchema] ,
                                        c.CHARACTER_SET_NAME [CharacterSetName] ,
                                        CAST(0 AS BIT) AS [IsMultiSet] ,
                                        CAST(COLUMNPROPERTY(OBJECT_ID(QUOTENAME(c.TABLE_SCHEMA)
                                                              + '.'
                                                              + QUOTENAME(c.TABLE_NAME)),
                                                            c.COLUMN_NAME,
                                                            'IsIdentity') AS BIT) AS [IsIdentity] ,
                                        CAST(COLUMNPROPERTY(OBJECT_ID(QUOTENAME(c.TABLE_SCHEMA)
                                                              + '.'
                                                              + QUOTENAME(c.TABLE_NAME)),
                                                            c.COLUMN_NAME,
                                                            'IsComputed')
                                        | CASE WHEN c.DATA_TYPE = 'timestamp'
                                               THEN 1
                                               ELSE 0
                                          END AS BIT) AS [IsStoreGenerated] ,
                                        c.COLUMN_DEFAULT [Default]
                              FROM      INFORMATION_SCHEMA.COLUMNS c
                                        INNER JOIN INFORMATION_SCHEMA.VIEWS v ON c.TABLE_CATALOG = v.TABLE_CATALOG
                                                              AND c.TABLE_SCHEMA = v.TABLE_SCHEMA
                                                              AND c.TABLE_NAME = v.TABLE_NAME
                              WHERE     NOT ( v.TABLE_SCHEMA = 'dbo'
                                              AND v.TABLE_NAME IN (
                                              'syssegments', 'sysconstraints' )
                                              AND SUBSTRING(CAST(SERVERPROPERTY('productversion') AS VARCHAR(20)),
                                                            1, 1) = 8
                                            )
                            ) AS [Extent3]
                   ) AS [UnionAll1] ON ( 0 = [UnionAll1].[C1] )
                                       AND ( [Extent1].[Id] = [UnionAll1].[ParentId] )
        LEFT OUTER JOIN ( SELECT    [UnionAll2].[Id] AS [C1] ,
                                    CAST(1 AS BIT) AS [C2]
                          FROM      ( SELECT    QUOTENAME(tc.CONSTRAINT_SCHEMA)
                                                + QUOTENAME(tc.CONSTRAINT_NAME) [Id] ,
                                                QUOTENAME(tc.TABLE_SCHEMA)
                                                + QUOTENAME(tc.TABLE_NAME) [ParentId] ,
                                                tc.CONSTRAINT_NAME [Name] ,
                                                tc.CONSTRAINT_TYPE [ConstraintType] ,
                                                CAST(CASE tc.IS_DEFERRABLE
                                                       WHEN 'NO' THEN 0
                                                       ELSE 1
                                                     END AS BIT) [IsDeferrable] ,
                                                CAST(CASE tc.INITIALLY_DEFERRED
                                                       WHEN 'NO' THEN 0
                                                       ELSE 1
                                                     END AS BIT) [IsInitiallyDeferred]
                                      FROM      INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc
                                      WHERE     tc.TABLE_NAME IS NOT NULL
                                    ) AS [Extent4]
                                    INNER JOIN ( SELECT 7 AS [C1] ,
                                                        [Extent5].[ConstraintId] AS [ConstraintId] ,
                                                        [Extent6].[Id] AS [Id]
                                                 FROM   ( SELECT
                                                              QUOTENAME(CONSTRAINT_SCHEMA)
                                                              + QUOTENAME(CONSTRAINT_NAME) [ConstraintId] ,
                                                              QUOTENAME(TABLE_SCHEMA)
                                                              + QUOTENAME(TABLE_NAME)
                                                              + QUOTENAME(COLUMN_NAME) [ColumnId]
                                                          FROM
                                                              INFORMATION_SCHEMA.KEY_COLUMN_USAGE
                                                        ) AS [Extent5]
                                                        INNER JOIN ( SELECT
                                                              QUOTENAME(c.TABLE_SCHEMA)
                                                              + QUOTENAME(c.TABLE_NAME)
                                                              + QUOTENAME(c.COLUMN_NAME) [Id] ,
                                                              QUOTENAME(c.TABLE_SCHEMA)
                                                              + QUOTENAME(c.TABLE_NAME) [ParentId] ,
                                                              c.COLUMN_NAME [Name] ,
                                                              c.ORDINAL_POSITION [Ordinal] ,
                                                              CAST(CASE c.IS_NULLABLE
                                                              WHEN 'YES'
                                                              THEN 1
                                                              WHEN 'NO' THEN 0
                                                              ELSE 0
                                                              END AS BIT) [IsNullable] ,
                                                              CASE
                                                              WHEN c.DATA_TYPE IN (
                                                              'varchar',
                                                              'nvarchar',
                                                              'varbinary' )
                                                              AND c.CHARACTER_MAXIMUM_LENGTH = -1
                                                              THEN c.DATA_TYPE
                                                              + '(max)'
                                                              ELSE c.DATA_TYPE
                                                              END AS [TypeName] ,
                                                              c.CHARACTER_MAXIMUM_LENGTH [MaxLength] ,
                                                              CAST(c.NUMERIC_PRECISION AS INTEGER) [Precision] ,
                                                              CAST(c.DATETIME_PRECISION AS INTEGER) [DateTimePrecision] ,
                                                              CAST(c.NUMERIC_SCALE AS INTEGER) [Scale] ,
                                                              c.COLLATION_CATALOG [CollationCatalog] ,
                                                              c.COLLATION_SCHEMA [CollationSchema] ,
                                                              c.COLLATION_NAME [CollationName] ,
                                                              c.CHARACTER_SET_CATALOG [CharacterSetCatalog] ,
                                                              c.CHARACTER_SET_SCHEMA [CharacterSetSchema] ,
                                                              c.CHARACTER_SET_NAME [CharacterSetName] ,
                                                              CAST(0 AS BIT) AS [IsMultiSet] ,
                                                              CAST(COLUMNPROPERTY(OBJECT_ID(QUOTENAME(c.TABLE_SCHEMA)
                                                              + '.'
                                                              + QUOTENAME(c.TABLE_NAME)),
                                                              c.COLUMN_NAME,
                                                              'IsIdentity') AS BIT) AS [IsIdentity] ,
                                                              CAST(COLUMNPROPERTY(OBJECT_ID(QUOTENAME(c.TABLE_SCHEMA)
                                                              + '.'
                                                              + QUOTENAME(c.TABLE_NAME)),
                                                              c.COLUMN_NAME,
                                                              'IsComputed')
                                                              | CASE
                                                              WHEN c.DATA_TYPE = 'timestamp'
                                                              THEN 1
                                                              ELSE 0
                                                              END AS BIT) AS [IsStoreGenerated] ,
                                                              c.COLUMN_DEFAULT AS [Default]
                                                              FROM
                                                              INFORMATION_SCHEMA.COLUMNS c
                                                              INNER JOIN INFORMATION_SCHEMA.TABLES t ON c.TABLE_CATALOG = t.TABLE_CATALOG
                                                              AND c.TABLE_SCHEMA = t.TABLE_SCHEMA
                                                              AND c.TABLE_NAME = t.TABLE_NAME
                                                              AND t.TABLE_TYPE = 'BASE TABLE'
                                                              ) AS [Extent6] ON [Extent6].[Id] = [Extent5].[ColumnId]
                                                 UNION ALL
                                                 SELECT 11 AS [C1] ,
                                                        [Extent7].[ConstraintId] AS [ConstraintId] ,
                                                        [Extent8].[Id] AS [Id]
                                                 FROM   ( SELECT
                                                              CAST(NULL AS NVARCHAR(1)) [ConstraintId] ,
                                                              CAST(NULL AS NVARCHAR(MAX)) [ColumnId]
                                                          WHERE
                                                              1 = 2
                                                        ) AS [Extent7]
                                                        INNER JOIN ( SELECT
                                                              QUOTENAME(c.TABLE_SCHEMA)
                                                              + QUOTENAME(c.TABLE_NAME)
                                                              + QUOTENAME(c.COLUMN_NAME) [Id] ,
                                                              QUOTENAME(c.TABLE_SCHEMA)
                                                              + QUOTENAME(c.TABLE_NAME) [ParentId] ,
                                                              c.COLUMN_NAME [Name] ,
                                                              ( SELECT TOP 1
                                                              VALUE
                                                              FROM
                                                              ::
                                                              FN_LISTEXTENDEDPROPERTY(N'MS_Description',
                                                              N'SCHEMA',
                                                              c.TABLE_SCHEMA,
                                                              N'TABLE',
                                                              c.TABLE_NAME,
                                                              N'COLUMN',
                                                              c.COLUMN_NAME)
                                                              ) AS [Description] ,
                                                              c.ORDINAL_POSITION [Ordinal] ,
                                                              CAST(CASE c.IS_NULLABLE
                                                              WHEN 'YES'
                                                              THEN 1
                                                              WHEN 'NO' THEN 0
                                                              ELSE 0
                                                              END AS BIT) [IsNullable] ,
                                                              CASE
                                                              WHEN c.DATA_TYPE IN (
                                                              'varchar',
                                                              'nvarchar',
                                                              'varbinary' )
                                                              AND c.CHARACTER_MAXIMUM_LENGTH = -1
                                                              THEN c.DATA_TYPE
                                                              + '(max)'
                                                              ELSE c.DATA_TYPE
                                                              END AS [TypeName] ,
                                                              c.CHARACTER_MAXIMUM_LENGTH [MaxLength] ,
                                                              CAST(c.NUMERIC_PRECISION AS INTEGER) [Precision] ,
                                                              CAST(c.DATETIME_PRECISION AS INTEGER) AS [DateTimePrecision] ,
                                                              CAST(c.NUMERIC_SCALE AS INTEGER) [Scale] ,
                                                              c.COLLATION_CATALOG [CollationCatalog] ,
                                                              c.COLLATION_SCHEMA [CollationSchema] ,
                                                              c.COLLATION_NAME [CollationName] ,
                                                              c.CHARACTER_SET_CATALOG [CharacterSetCatalog] ,
                                                              c.CHARACTER_SET_SCHEMA [CharacterSetSchema] ,
                                                              c.CHARACTER_SET_NAME [CharacterSetName] ,
                                                              CAST(0 AS BIT) AS [IsMultiSet] ,
                                                              CAST(COLUMNPROPERTY(OBJECT_ID(QUOTENAME(c.TABLE_SCHEMA)
                                                              + '.'
                                                              + QUOTENAME(c.TABLE_NAME)),
                                                              c.COLUMN_NAME,
                                                              'IsIdentity') AS BIT) AS [IsIdentity] ,
                                                              CAST(COLUMNPROPERTY(OBJECT_ID(QUOTENAME(c.TABLE_SCHEMA)
                                                              + '.'
                                                              + QUOTENAME(c.TABLE_NAME)),
                                                              c.COLUMN_NAME,
                                                              'IsComputed')
                                                              | CASE
                                                              WHEN c.DATA_TYPE = 'timestamp'
                                                              THEN 1
                                                              ELSE 0
                                                              END AS BIT) AS [IsStoreGenerated] ,
                                                              c.COLUMN_DEFAULT [Default]
                                                              FROM
                                                              INFORMATION_SCHEMA.COLUMNS c
                                                              INNER JOIN INFORMATION_SCHEMA.VIEWS v ON c.TABLE_CATALOG = v.TABLE_CATALOG
                                                              AND c.TABLE_SCHEMA = v.TABLE_SCHEMA
                                                              AND c.TABLE_NAME = v.TABLE_NAME
                                                              WHERE
                                                              NOT ( v.TABLE_SCHEMA = 'dbo'
                                                              AND v.TABLE_NAME IN (
                                                              'syssegments',
                                                              'sysconstraints' )
                                                              AND SUBSTRING(CAST(SERVERPROPERTY('productversion') AS VARCHAR(20)),
                                                              1, 1) = 8
                                                              )
                                                              ) AS [Extent8] ON [Extent8].[Id] = [Extent7].[ColumnId]
                                               ) AS [UnionAll2] ON ( 7 = [UnionAll2].[C1] )
                                                              AND ( [Extent4].[Id] = [UnionAll2].[ConstraintId] )
                          WHERE     [Extent4].[ConstraintType] = N'PRIMARY KEY'
                        ) AS [Project5] ON [UnionAll1].[Id] = [Project5].[C1]
WHERE   [Extent1].[Name] LIKE '%'";
            #endregion
            this.SqlServerTablesSelect =
            #region SqlServerTablesSelect
 @"SELECT  [Project1].[Id] AS [Id] ,
        [Project1].[CatalogName] AS [CatalogName] ,
        [Project1].[SchemaName] AS [SchemaName] ,
        [Project1].[Name] AS [Name] ,
        [Project1].[Description] AS [Description]
FROM    ( SELECT    [Extent1].[Id] AS [Id] ,
                    [Extent1].[CatalogName] AS [CatalogName] ,
                    [Extent1].[SchemaName] AS [SchemaName] ,
                    [Extent1].[Name] AS [Name] ,
                    [Extent1].[Description] AS [Description]
          FROM      ( SELECT    QUOTENAME(TABLE_SCHEMA) + QUOTENAME(TABLE_NAME) [Id] ,
                                TABLE_CATALOG [CatalogName] ,
                                TABLE_SCHEMA [SchemaName] ,
                                TABLE_NAME [Name] ,
                                ( SELECT    CAST(VALUE AS NVARCHAR(MAX))
                                  FROM      ::
                                            FN_LISTEXTENDEDPROPERTY(N'MS_Description',
                                                              N'SCHEMA',
                                                              TABLE_SCHEMA,
                                                              N'TABLE',
                                                              TABLE_NAME,
                                                              DEFAULT, DEFAULT)
                                ) AS [Description]
                      FROM      INFORMATION_SCHEMA.TABLES
                      WHERE     TABLE_TYPE = 'BASE TABLE'
                    ) AS [Extent1]
        ) AS [Project1]";
            #endregion
            this.SqlServerViewColumnsSelect =
            #region SqlServerViewColumnsSelect
 @"SELECT  [UnionAll1].[Id] AS [Id] ,
        [UnionAll1].[Ordinal] AS [Ordinal] ,
        [Extent1].[CatalogName] AS [CatalogName] ,
        [Extent1].[SchemaName] AS [SchemaName] ,
        [Extent1].[Name] AS [ViewName] ,
        [UnionAll1].[Name] AS [Name] ,
        [UnionAll1].[Description] AS [Description] ,
        [UnionAll1].[IsNullable] AS [IsNullable] ,
        [UnionAll1].[TypeName] AS [TypeName] ,
        [UnionAll1].[MaxLength] AS [MaxLength] ,
        [UnionAll1].[Precision] AS [Precision] ,
        [UnionAll1].[DateTimePrecision] AS [DateTimePrecision] ,
        [UnionAll1].[Scale] AS [Scale] ,
        [UnionAll1].[IsIdentity] AS [IsIdentity] ,
        [UnionAll1].[IsStoreGenerated] AS [IsStoreGenerated] ,
        CASE WHEN ( [Project5].[C2] IS NULL ) THEN CAST(0 AS BIT)
             ELSE [Project5].[C2]
        END AS [IsPrimaryKey]
FROM    ( SELECT    QUOTENAME(TABLE_SCHEMA) + QUOTENAME(TABLE_NAME) [Id] ,
                    TABLE_CATALOG [CatalogName] ,
                    TABLE_SCHEMA [SchemaName] ,
                    TABLE_NAME [Name] ,
                    VIEW_DEFINITION [ViewDefinition] ,
                    CAST(CASE IS_UPDATABLE
                           WHEN 'YES' THEN 1
                           WHEN 'NO' THEN 0
                           ELSE 0
                         END AS BIT) [IsUpdatable]
          FROM      INFORMATION_SCHEMA.VIEWS
          WHERE     NOT ( TABLE_SCHEMA = 'dbo'
                          AND TABLE_NAME IN ( 'syssegments', 'sysconstraints' )
                          AND SUBSTRING(CAST(SERVERPROPERTY('productversion') AS VARCHAR(20)),
                                        1, 1) = 8
                        )
        ) AS [Extent1]
        INNER JOIN ( SELECT [Extent2].[Id] AS [Id] ,
                            [Extent2].[Name] AS [Name] ,
                            [Extent2].[Description] AS [Description] ,
                            [Extent2].[Ordinal] AS [Ordinal] ,
                            [Extent2].[IsNullable] AS [IsNullable] ,
                            [Extent2].[TypeName] AS [TypeName] ,
                            [Extent2].[MaxLength] AS [MaxLength] ,
                            [Extent2].[Precision] AS [Precision] ,
                            [Extent2].[DateTimePrecision] AS [DateTimePrecision] ,
                            [Extent2].[Scale] AS [Scale] ,
                            [Extent2].[IsIdentity] AS [IsIdentity] ,
                            [Extent2].[IsStoreGenerated] AS [IsStoreGenerated] ,
                            4 AS [C1] ,
                            [Extent2].[ParentId] AS [ParentId]
                     FROM   ( SELECT    QUOTENAME(c.TABLE_SCHEMA)
                                        + QUOTENAME(c.TABLE_NAME)
                                        + QUOTENAME(c.COLUMN_NAME) [Id] ,
                                        QUOTENAME(c.TABLE_SCHEMA)
                                        + QUOTENAME(c.TABLE_NAME) [ParentId] ,
                                        c.COLUMN_NAME [Name] ,
                                        ( SELECT    CAST(VALUE AS NVARCHAR(MAX))
                                          FROM      ::
                                                    FN_LISTEXTENDEDPROPERTY(N'MS_Description',
                                                              N'SCHEMA',
                                                              c.TABLE_SCHEMA,
                                                              N'VIEW',
                                                              c.TABLE_NAME,
                                                              N'COLUMN',
                                                              c.COLUMN_NAME)
                                        ) AS [Description] ,
                                        c.ORDINAL_POSITION [Ordinal] ,
                                        CAST(CASE c.IS_NULLABLE
                                               WHEN 'YES' THEN 1
                                               WHEN 'NO' THEN 0
                                               ELSE 0
                                             END AS BIT) [IsNullable] ,
                                        CASE WHEN c.DATA_TYPE IN ( 'varchar',
                                                              'nvarchar',
                                                              'varbinary' )
                                                  AND c.CHARACTER_MAXIMUM_LENGTH = -1
                                             THEN c.DATA_TYPE + '(max)'
                                             ELSE c.DATA_TYPE
                                        END AS [TypeName] ,
                                        c.CHARACTER_MAXIMUM_LENGTH [MaxLength] ,
                                        CAST(c.NUMERIC_PRECISION AS INTEGER) [Precision] ,
                                        CAST(c.DATETIME_PRECISION AS INTEGER) [DateTimePrecision] ,
                                        CAST(c.NUMERIC_SCALE AS INTEGER) [Scale] ,
                                        c.COLLATION_CATALOG [CollationCatalog] ,
                                        c.COLLATION_SCHEMA [CollationSchema] ,
                                        c.COLLATION_NAME [CollationName] ,
                                        c.CHARACTER_SET_CATALOG [CharacterSetCatalog] ,
                                        c.CHARACTER_SET_SCHEMA [CharacterSetSchema] ,
                                        c.CHARACTER_SET_NAME [CharacterSetName] ,
                                        CAST(0 AS BIT) AS [IsMultiSet] ,
                                        CAST(COLUMNPROPERTY(OBJECT_ID(QUOTENAME(c.TABLE_SCHEMA)
                                                              + '.'
                                                              + QUOTENAME(c.TABLE_NAME)),
                                                            c.COLUMN_NAME,
                                                            'IsIdentity') AS BIT) AS [IsIdentity] ,
                                        CAST(COLUMNPROPERTY(OBJECT_ID(QUOTENAME(c.TABLE_SCHEMA)
                                                              + '.'
                                                              + QUOTENAME(c.TABLE_NAME)),
                                                            c.COLUMN_NAME,
                                                            'IsComputed')
                                        | CASE WHEN c.DATA_TYPE = 'timestamp'
                                               THEN 1
                                               ELSE 0
                                          END AS BIT) AS [IsStoreGenerated] ,
                                        c.COLUMN_DEFAULT AS [Default]
                              FROM      INFORMATION_SCHEMA.COLUMNS c
                                        INNER JOIN INFORMATION_SCHEMA.TABLES t ON c.TABLE_CATALOG = t.TABLE_CATALOG
                                                              AND c.TABLE_SCHEMA = t.TABLE_SCHEMA
                                                              AND c.TABLE_NAME = t.TABLE_NAME
                                                              AND t.TABLE_TYPE = 'BASE TABLE'
                            ) AS [Extent2]
                     UNION ALL
                     SELECT [Extent3].[Id] AS [Id] ,
                            [Extent3].[Name] AS [Name] ,
                            [Extent3].[Description] AS [Description] ,
                            [Extent3].[Ordinal] AS [Ordinal] ,
                            [Extent3].[IsNullable] AS [IsNullable] ,
                            [Extent3].[TypeName] AS [TypeName] ,
                            [Extent3].[MaxLength] AS [MaxLength] ,
                            [Extent3].[Precision] AS [Precision] ,
                            [Extent3].[DateTimePrecision] AS [DateTimePrecision] ,
                            [Extent3].[Scale] AS [Scale] ,
                            [Extent3].[IsIdentity] AS [IsIdentity] ,
                            [Extent3].[IsStoreGenerated] AS [IsStoreGenerated] ,
                            0 AS [C1] ,
                            [Extent3].[ParentId] AS [ParentId]
                     FROM   ( SELECT    QUOTENAME(c.TABLE_SCHEMA)
                                        + QUOTENAME(c.TABLE_NAME)
                                        + QUOTENAME(c.COLUMN_NAME) [Id] ,
                                        QUOTENAME(c.TABLE_SCHEMA)
                                        + QUOTENAME(c.TABLE_NAME) [ParentId] ,
                                        c.COLUMN_NAME [Name] ,
                                        ( SELECT    CAST(VALUE AS NVARCHAR(MAX))
                                          FROM      ::
                                                    FN_LISTEXTENDEDPROPERTY(N'MS_Description',
                                                              N'SCHEMA',
                                                              c.TABLE_SCHEMA,
                                                              N'VIEW',
                                                              c.TABLE_NAME,
                                                              N'COLUMN',
                                                              c.COLUMN_NAME)
                                        ) AS [Description] ,
                                        c.ORDINAL_POSITION [Ordinal] ,
                                        CAST(CASE c.IS_NULLABLE
                                               WHEN 'YES' THEN 1
                                               WHEN 'NO' THEN 0
                                               ELSE 0
                                             END AS BIT) [IsNullable] ,
                                        CASE WHEN c.DATA_TYPE IN ( 'varchar',
                                                              'nvarchar',
                                                              'varbinary' )
                                                  AND c.CHARACTER_MAXIMUM_LENGTH = -1
                                             THEN c.DATA_TYPE + '(max)'
                                             ELSE c.DATA_TYPE
                                        END AS [TypeName] ,
                                        c.CHARACTER_MAXIMUM_LENGTH [MaxLength] ,
                                        CAST(c.NUMERIC_PRECISION AS INTEGER) [Precision] ,
                                        CAST(c.DATETIME_PRECISION AS INTEGER) AS [DateTimePrecision] ,
                                        CAST(c.NUMERIC_SCALE AS INTEGER) [Scale] ,
                                        c.COLLATION_CATALOG [CollationCatalog] ,
                                        c.COLLATION_SCHEMA [CollationSchema] ,
                                        c.COLLATION_NAME [CollationName] ,
                                        c.CHARACTER_SET_CATALOG [CharacterSetCatalog] ,
                                        c.CHARACTER_SET_SCHEMA [CharacterSetSchema] ,
                                        c.CHARACTER_SET_NAME [CharacterSetName] ,
                                        CAST(0 AS BIT) AS [IsMultiSet] ,
                                        CAST(COLUMNPROPERTY(OBJECT_ID(QUOTENAME(c.TABLE_SCHEMA)
                                                              + '.'
                                                              + QUOTENAME(c.TABLE_NAME)),
                                                            c.COLUMN_NAME,
                                                            'IsIdentity') AS BIT) AS [IsIdentity] ,
                                        CAST(COLUMNPROPERTY(OBJECT_ID(QUOTENAME(c.TABLE_SCHEMA)
                                                              + '.'
                                                              + QUOTENAME(c.TABLE_NAME)),
                                                            c.COLUMN_NAME,
                                                            'IsComputed')
                                        | CASE WHEN c.DATA_TYPE = 'timestamp'
                                               THEN 1
                                               ELSE 0
                                          END AS BIT) AS [IsStoreGenerated] ,
                                        c.COLUMN_DEFAULT [Default]
                              FROM      INFORMATION_SCHEMA.COLUMNS c
                                        INNER JOIN INFORMATION_SCHEMA.VIEWS v ON c.TABLE_CATALOG = v.TABLE_CATALOG
                                                              AND c.TABLE_SCHEMA = v.TABLE_SCHEMA
                                                              AND c.TABLE_NAME = v.TABLE_NAME
                              WHERE     NOT ( v.TABLE_SCHEMA = 'dbo'
                                              AND v.TABLE_NAME IN (
                                              'syssegments', 'sysconstraints' )
                                              AND SUBSTRING(CAST(SERVERPROPERTY('productversion') AS VARCHAR(20)),
                                                            1, 1) = 8
                                            )
                            ) AS [Extent3]
                   ) AS [UnionAll1] ON ( 0 = [UnionAll1].[C1] )
                                       AND ( [Extent1].[Id] = [UnionAll1].[ParentId] )
        LEFT OUTER JOIN ( SELECT    [UnionAll2].[Id] AS [C1] ,
                                    CAST(1 AS BIT) AS [C2]
                          FROM      ( SELECT    CAST(NULL AS NVARCHAR(1)) [Id] ,
                                                CAST(NULL AS NVARCHAR(256)) [ParentId] ,
                                                CAST(NULL AS NVARCHAR(256)) [Name] ,
                                                CAST(NULL AS NVARCHAR(256)) [ConstraintType] ,
                                                CAST(0 AS BIT) [IsDeferrable] ,
                                                CAST(0 AS BIT) [IsInitiallyDeferred] ,
                                                CAST(NULL AS NVARCHAR(MAX)) [Expression] ,
                                                CAST(NULL AS NVARCHAR(11)) [UpdateRule] ,
                                                CAST(NULL AS NVARCHAR(11)) [DeleteRule]
                                      WHERE     1 = 2
                                    ) AS [Extent4]
                                    INNER JOIN ( SELECT 10 AS [C1] ,
                                                        [Extent5].[ConstraintId] AS [ConstraintId] ,
                                                        [Extent6].[Id] AS [Id]
                                                 FROM   ( SELECT
                                                              QUOTENAME(CONSTRAINT_SCHEMA)
                                                              + QUOTENAME(CONSTRAINT_NAME) [ConstraintId] ,
                                                              QUOTENAME(TABLE_SCHEMA)
                                                              + QUOTENAME(TABLE_NAME)
                                                              + QUOTENAME(COLUMN_NAME) [ColumnId]
                                                          FROM
                                                              INFORMATION_SCHEMA.KEY_COLUMN_USAGE
                                                        ) AS [Extent5]
                                                        INNER JOIN ( SELECT
                                                              QUOTENAME(c.TABLE_SCHEMA)
                                                              + QUOTENAME(c.TABLE_NAME)
                                                              + QUOTENAME(c.COLUMN_NAME) [Id] ,
                                                              QUOTENAME(c.TABLE_SCHEMA)
                                                              + QUOTENAME(c.TABLE_NAME) [ParentId] ,
                                                              c.COLUMN_NAME [Name] ,
                                                              c.ORDINAL_POSITION [Ordinal] ,
                                                              CAST(CASE c.IS_NULLABLE
                                                              WHEN 'YES'
                                                              THEN 1
                                                              WHEN 'NO' THEN 0
                                                              ELSE 0
                                                              END AS BIT) [IsNullable] ,
                                                              CASE
                                                              WHEN c.DATA_TYPE IN (
                                                              'varchar',
                                                              'nvarchar',
                                                              'varbinary' )
                                                              AND c.CHARACTER_MAXIMUM_LENGTH = -1
                                                              THEN c.DATA_TYPE
                                                              + '(max)'
                                                              ELSE c.DATA_TYPE
                                                              END AS [TypeName] ,
                                                              c.CHARACTER_MAXIMUM_LENGTH [MaxLength] ,
                                                              CAST(c.NUMERIC_PRECISION AS INTEGER) [Precision] ,
                                                              CAST(c.DATETIME_PRECISION AS INTEGER) [DateTimePrecision] ,
                                                              CAST(c.NUMERIC_SCALE AS INTEGER) [Scale] ,
                                                              c.COLLATION_CATALOG [CollationCatalog] ,
                                                              c.COLLATION_SCHEMA [CollationSchema] ,
                                                              c.COLLATION_NAME [CollationName] ,
                                                              c.CHARACTER_SET_CATALOG [CharacterSetCatalog] ,
                                                              c.CHARACTER_SET_SCHEMA [CharacterSetSchema] ,
                                                              c.CHARACTER_SET_NAME [CharacterSetName] ,
                                                              CAST(0 AS BIT) AS [IsMultiSet] ,
                                                              CAST(COLUMNPROPERTY(OBJECT_ID(QUOTENAME(c.TABLE_SCHEMA)
                                                              + '.'
                                                              + QUOTENAME(c.TABLE_NAME)),
                                                              c.COLUMN_NAME,
                                                              'IsIdentity') AS BIT) AS [IsIdentity] ,
                                                              CAST(COLUMNPROPERTY(OBJECT_ID(QUOTENAME(c.TABLE_SCHEMA)
                                                              + '.'
                                                              + QUOTENAME(c.TABLE_NAME)),
                                                              c.COLUMN_NAME,
                                                              'IsComputed')
                                                              | CASE
                                                              WHEN c.DATA_TYPE = 'timestamp'
                                                              THEN 1
                                                              ELSE 0
                                                              END AS BIT) AS [IsStoreGenerated] ,
                                                              c.COLUMN_DEFAULT AS [Default]
                                                              FROM
                                                              INFORMATION_SCHEMA.COLUMNS c
                                                              INNER JOIN INFORMATION_SCHEMA.TABLES t ON c.TABLE_CATALOG = t.TABLE_CATALOG
                                                              AND c.TABLE_SCHEMA = t.TABLE_SCHEMA
                                                              AND c.TABLE_NAME = t.TABLE_NAME
                                                              AND t.TABLE_TYPE = 'BASE TABLE'
                                                              ) AS [Extent6] ON [Extent6].[Id] = [Extent5].[ColumnId]
                                                 UNION ALL
                                                 SELECT 7 AS [C1] ,
                                                        [Extent7].[ConstraintId] AS [ConstraintId] ,
                                                        [Extent8].[Id] AS [Id]
                                                 FROM   ( SELECT
                                                              CAST(NULL AS NVARCHAR(1)) [ConstraintId] ,
                                                              CAST(NULL AS NVARCHAR(MAX)) [ColumnId]
                                                          WHERE
                                                              1 = 2
                                                        ) AS [Extent7]
                                                        INNER JOIN ( SELECT
                                                              QUOTENAME(c.TABLE_SCHEMA)
                                                              + QUOTENAME(c.TABLE_NAME)
                                                              + QUOTENAME(c.COLUMN_NAME) [Id] ,
                                                              QUOTENAME(c.TABLE_SCHEMA)
                                                              + QUOTENAME(c.TABLE_NAME) [ParentId] ,
                                                              c.COLUMN_NAME [Name] ,
                                                              c.ORDINAL_POSITION [Ordinal] ,
                                                              CAST(CASE c.IS_NULLABLE
                                                              WHEN 'YES'
                                                              THEN 1
                                                              WHEN 'NO' THEN 0
                                                              ELSE 0
                                                              END AS BIT) [IsNullable] ,
                                                              CASE
                                                              WHEN c.DATA_TYPE IN (
                                                              'varchar',
                                                              'nvarchar',
                                                              'varbinary' )
                                                              AND c.CHARACTER_MAXIMUM_LENGTH = -1
                                                              THEN c.DATA_TYPE
                                                              + '(max)'
                                                              ELSE c.DATA_TYPE
                                                              END AS [TypeName] ,
                                                              c.CHARACTER_MAXIMUM_LENGTH [MaxLength] ,
                                                              CAST(c.NUMERIC_PRECISION AS INTEGER) [Precision] ,
                                                              CAST(c.DATETIME_PRECISION AS INTEGER) AS [DateTimePrecision] ,
                                                              CAST(c.NUMERIC_SCALE AS INTEGER) [Scale] ,
                                                              c.COLLATION_CATALOG [CollationCatalog] ,
                                                              c.COLLATION_SCHEMA [CollationSchema] ,
                                                              c.COLLATION_NAME [CollationName] ,
                                                              c.CHARACTER_SET_CATALOG [CharacterSetCatalog] ,
                                                              c.CHARACTER_SET_SCHEMA [CharacterSetSchema] ,
                                                              c.CHARACTER_SET_NAME [CharacterSetName] ,
                                                              CAST(0 AS BIT) AS [IsMultiSet] ,
                                                              CAST(COLUMNPROPERTY(OBJECT_ID(QUOTENAME(c.TABLE_SCHEMA)
                                                              + '.'
                                                              + QUOTENAME(c.TABLE_NAME)),
                                                              c.COLUMN_NAME,
                                                              'IsIdentity') AS BIT) AS [IsIdentity] ,
                                                              CAST(COLUMNPROPERTY(OBJECT_ID(QUOTENAME(c.TABLE_SCHEMA)
                                                              + '.'
                                                              + QUOTENAME(c.TABLE_NAME)),
                                                              c.COLUMN_NAME,
                                                              'IsComputed')
                                                              | CASE
                                                              WHEN c.DATA_TYPE = 'timestamp'
                                                              THEN 1
                                                              ELSE 0
                                                              END AS BIT) AS [IsStoreGenerated] ,
                                                              c.COLUMN_DEFAULT [Default]
                                                              FROM
                                                              INFORMATION_SCHEMA.COLUMNS c
                                                              INNER JOIN INFORMATION_SCHEMA.VIEWS v ON c.TABLE_CATALOG = v.TABLE_CATALOG
                                                              AND c.TABLE_SCHEMA = v.TABLE_SCHEMA
                                                              AND c.TABLE_NAME = v.TABLE_NAME
                                                              WHERE
                                                              NOT ( v.TABLE_SCHEMA = 'dbo'
                                                              AND v.TABLE_NAME IN (
                                                              'syssegments',
                                                              'sysconstraints' )
                                                              AND SUBSTRING(CAST(SERVERPROPERTY('productversion') AS VARCHAR(20)),
                                                              1, 1) = 8
                                                              )
                                                              ) AS [Extent8] ON [Extent8].[Id] = [Extent7].[ColumnId]
                                               ) AS [UnionAll2] ON ( 7 = [UnionAll2].[C1] )
                                                              AND ( [Extent4].[Id] = [UnionAll2].[ConstraintId] )
                          WHERE     [Extent4].[ConstraintType] = N'PRIMARY KEY'
                        ) AS [Project5] ON [UnionAll1].[Id] = [Project5].[C1]
WHERE   [Extent1].[Name] LIKE '%'";
            #endregion
            this.SqlServerViewsSelect =
            #region SqlServerViewsSelect
 @"SELECT  [Project1].[Id] AS [Id] ,
        [Project1].[CatalogName] AS [CatalogName] ,
        [Project1].[SchemaName] AS [SchemaName] ,
        [Project1].[Name] AS [Name] ,
        [Project1].[Description] AS [Description]
FROM    ( SELECT    [Extent1].[Id] AS [Id] ,
                    [Extent1].[CatalogName] AS [CatalogName] ,
                    [Extent1].[SchemaName] AS [SchemaName] ,
                    [Extent1].[Name] AS [Name] ,
                    [Extent1].[Description] AS [Description]
          FROM      ( SELECT    QUOTENAME(TABLE_SCHEMA) + QUOTENAME(TABLE_NAME) [Id] ,
                                TABLE_CATALOG [CatalogName] ,
                                TABLE_SCHEMA [SchemaName] ,
                                TABLE_NAME [Name] ,
                                ( SELECT    CAST(VALUE AS NVARCHAR(MAX))
                                  FROM      ::
                                            FN_LISTEXTENDEDPROPERTY(N'MS_Description',
                                                              N'SCHEMA',
                                                              TABLE_SCHEMA,
                                                              N'VIEW',
                                                              TABLE_NAME,
                                                              DEFAULT, DEFAULT)
                                ) AS [Description] ,
                                VIEW_DEFINITION [ViewDefinition] ,
                                CAST(CASE IS_UPDATABLE
                                       WHEN 'YES' THEN 1
                                       WHEN 'NO' THEN 0
                                       ELSE 0
                                     END AS BIT) [IsUpdatable]
                      FROM      INFORMATION_SCHEMA.VIEWS
                      WHERE     NOT ( TABLE_SCHEMA = 'dbo'
                                      AND TABLE_NAME IN ( 'syssegments',
                                                          'sysconstraints' )
                                      AND SUBSTRING(CAST(SERVERPROPERTY('productversion') AS VARCHAR(20)),
                                                    1, 1) = 8
                                    )
                    ) AS [Extent1]
        ) AS [Project1]";
            #endregion
            this.TicksTimeout = 180;
            this.InfoFormat = "json";
            this.EntityArchivePath = Path.Combine(dir, "db", "Archive");
            this.EntityBackupPath = Path.Combine(dir, "db", "Backup");
            this.ServiceIsAlive = true;
            this.TraceIsEnabled = false;
            this.BeatPeriod = 5;
            this.CenterNodeId = "e16ef438-0f95-4605-8556-2ae6e10f1240";
            this.ThisNodeId = "e16ef438-0f95-4605-8556-2ae6e10f1240";
            this.AuditLevel = ConfigLevel.Level5CatalogAction;
            this.ImplicitAudit = AuditType.NotAudit;
            this.AclLevel = ConfigLevel.Level5CatalogAction;
            this.ImplicitAllow = AllowType.ExplicitAllow;
            this.EntityLogonLevel = ConfigLevel.Level5CatalogAction;
            this.ImplicitEntityLogon = EntityLogon.ExplicitLogon;
        }

        public Func<PluginType, string> PluginBaseDirectory { get; set; }

        public string CurrentAcSessionCacheKey { get; set; }

        /// <summary>
        /// 管道插件
        /// </summary>
        public List<IPlugin> Plugins { get; set; }

        public bool EnableClientCache { get; set; }
        public bool EnableOperationLog { get; set; }
        public string SelfAppSystemCode { get; set; }
        public string SqlServerTableColumnsSelect { get; set; }
        public string SqlServerTablesSelect { get; set; }
        public string SqlServerViewColumnsSelect { get; set; }
        public string SqlServerViewsSelect { get; set; }
        public int TicksTimeout { get; set; }

        public string SequenceIdGenerator { get; set; }

        public string IdGenerator { get; set; }

        /// <summary>
        /// 命令的InfoID和InfoValue字符串的格式。该格式是命令持久化所使用的默认格式。
        /// </summary>
        public string InfoFormat { get; set; }

        /// <summary>
        /// 实体库归档路径
        /// </summary>
        public string EntityArchivePath { get; set; }

        /// <summary>
        /// 实体库备份路径
        /// </summary>
        public string EntityBackupPath { get; set; }

        /// <summary>
        /// 服务是否可用
        /// </summary>
        public bool ServiceIsAlive { get; set; }

        /// <summary>
        /// 是否开启Trace
        /// </summary>
        public bool TraceIsEnabled { get; set; }

        /// <summary>
        /// 检测节点是否在线的请求周期单位（分钟）
        /// </summary>
        public int BeatPeriod { get; set; }

        /// <summary>
        /// 中心节点标识
        /// </summary>
        public string CenterNodeId { get; set; }

        /// <summary>
        /// 本节点自我标识
        /// </summary>
        public string ThisNodeId { get; set; }

        /// <summary>
        /// 审核配置深度
        /// </summary>
        public ConfigLevel AuditLevel { get; set; }

        private AuditType _implicitAudit;
        /// <summary>
        /// 隐式审核意为
        /// </summary>
        public AuditType ImplicitAudit
        {
            get
            {
                return _implicitAudit;
            }
            set
            {
                if (value == AuditType.Invalid || value == AuditType.ImplicitAudit)
                {
                    throw new AnycmdException("审核类型配置错误，取值不能是：Invalid或" + AuditType.ImplicitAudit.ToName());
                }
                _implicitAudit = value;
            }
        }

        /// <summary>
        /// 访问控制配置深度
        /// </summary>
        public ConfigLevel AclLevel { get; set; }

        private AllowType _implicitAllow;
        /// <summary>
        /// 隐式访问控制意为
        /// </summary>
        public AllowType ImplicitAllow
        {
            get
            {
                return _implicitAllow;
            }
            set
            {
                if (value == AllowType.Invalid || value == AllowType.ImplicitAllow)
                {
                    throw new AnycmdException("访问控制类型配置错误，取值不能是：Invalid或" + AllowType.ImplicitAllow.ToName());
                }
                _implicitAllow = value;
            }
        }

        /// <summary>
        /// 实体登录控制配置深度
        /// </summary>
        public ConfigLevel EntityLogonLevel { get; set; }

        private EntityLogon _implicitEntityLogon;
        /// <summary>
        /// 隐式实体登录意为
        /// </summary>
        public EntityLogon ImplicitEntityLogon {
            get
            {
                return _implicitEntityLogon;
            }
            set
            {
                if (value == EntityLogon.Invalid || value == EntityLogon.ImplicitLogon)
                {
                    throw new AnycmdException("实体登录控制类型配置错误，取值不能是：Invalid或" + EntityLogon.ImplicitLogon.ToName());
                }
                _implicitEntityLogon = value;
            }
        }
    }
}
