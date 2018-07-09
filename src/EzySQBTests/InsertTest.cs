using System;
using System.Collections.Generic;
using EzySQB;
using EzySQB.Clauses;
using EzySQB.Statements;
using Xunit;

namespace EzySQBTests
{
    public class InsertTest : TestBase
    {

        [Fact]
        public void TestFluentValue()
        {
            Assert.Equal(
                "insert into categories (name, description, level) values (@name, @description, @level)",
                GetQueryBuilder()
                    .Insert("categories")
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
                "insert into categories (name, description, level) values (@name, @description, @level)",
                GetQueryBuilder()
                    .Insert("categories")
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
        public void TestFluentValueWithReturning()
        {
            Assert.Equal(
                "insert into categories (name, description, level) values (@name, @description, @level) returning id",
                GetQueryBuilder()
                    .Insert("categories")
                    .Value("name", "@name")
                    .Value("description", "@description")
                    .Value("level", "@level")
                    .Returning("id")
                    .ToString(),
                true
            );
        }

        [Fact]
        public void TestFluentValueWithReturningId()
        {
            Assert.Equal(
                "insert into categories (name, description, level) values (@name, @description, @level) returning id",
                GetQueryBuilder()
                    .Insert("categories")
                    .Value("name", "@name")
                    .Value("description", "@description")
                    .Value("level", "@level")
                    .ReturningId()
                    .ToString(),
                true
            );
        }

        [Fact]
        public void TestValuesWithMultipleReturning()
        {
            Assert.Equal(
                "insert into categories (name, description, level) values (@name, @description, @level) returning id, name, level",
                GetQueryBuilder()
                    .Insert("categories")
                    .Values(new Dictionary<string, dynamic> {
                        {"name", "@name"},
                        {"description", "@description"},
                        {"level", "@level"}
                    })
                    .Returning(new List<string> { "id", "name", "level" })
                    .ToString(),
                true
            );
        }

        [Fact]
        public void TestValuesWithMultipleReturningPrettified()
        {
            Assert.Equal(
                "insert into categories\n(name, description, level)\nvalues (@name, @description, @level)\nreturning id, name, level",
                GetQueryBuilder()
                    .Insert("categories")
                    .Values(new Dictionary<string, dynamic> {
                        {"name", "@name"},
                        {"description", "@description"},
                        {"level", "@level"}
                    })
                    .Returning(new List<string> { "id", "name", "level" })
                    .PrettyAssemble(),
                true
            );
        }

    }
}