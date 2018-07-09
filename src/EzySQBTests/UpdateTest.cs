using System;
using System.Collections.Generic;
using EzySQB;
using EzySQB.Clauses;
using EzySQB.Statements;
using Xunit;


namespace EzySQBTests
{
    public class UpdateTest : TestBase
    {

        [Fact]
        public void TestFluentValue()
        {
            Assert.Equal(
                "update from categories set name = @name, description = @description, level = @level",
                GetQueryBuilder()
                    .Update("categories")
                    .Value("name", "@name")
                    .Value("description", "@description")
                    .Value("level", "@level")
                    .ToString(),
                true
            );
        }

        [Fact]
        public void TestValues()
        {
            Assert.Equal(
               "update from categories set name = @name, description = @description, level = @level",
                GetQueryBuilder()
                    .Update("categories")
                    .Values(new Dictionary<string, dynamic> {
                        {"name", "@name"},
                        {"description", "@description"},
                        {"level", "@level"}
                    })
                    .ToString(),
                true
            );
        }

        [Fact]
        public void TestFluentValueWithConditions()
        {
            Assert.Equal(
                "update from categories set name = @name, description = @description, level = @level where id = @c_id and name like @c_name",
                GetQueryBuilder()
                    .Update("categories")
                    .Value("name", "@name")
                    .Value("description", "@description")
                    .Value("level", "@level")
                    .Where("id = @c_id")
                    .AndWhere("name like @c_name")
                    .ToString(),
                true
            );
        }

        [Fact]
        public void TestPrettyFluentValueWithConditions()
        {
            Assert.Equal(
                "update from categories\nset name = @name, description = @description, level = @level\nwhere id = @c_id and\n\tname like @c_name",
                GetQueryBuilder()
                    .Update("categories")
                    .Value("name", "@name")
                    .Value("description", "@description")
                    .Value("level", "@level")
                    .Where("id = @c_id")
                    .AndWhere("name like @c_name")
                    .PrettyAssemble(),
                true
            );
        }


    }
}