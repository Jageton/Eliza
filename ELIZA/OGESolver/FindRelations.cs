using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace OGESolver
{
    public class FindRelations: AbstractAlgorithm<IEnumerable<DataRow>>
    {
        protected DataTable people;
        protected DataTable relations;
        protected DataRow row;
        protected RelationType rel;

        public FindRelations(DataTable people, DataTable relations, DataRow row,
            RelationType rel)
        {
            this.people = people;
            this.relations = relations;
            this.row = row;
            this.rel = rel;
            name = "Find_Relations";
        }

        public override IEnumerable<DataRow> Execute()
        {
            switch (rel)
            {
                case(RelationType.Child):
                    return FindChildOf(row);
                case(RelationType.Parent):
                    return FindParentsOf(row);
                case(RelationType.Son):
                    return Males(FindChildOf(row));
                case(RelationType.Daughter):
                    return Females(FindChildOf(row));
                case (RelationType.Father):
                    return Males(FindParentsOf(row));
                case(RelationType.Mother):
                    return Females(FindParentsOf(row));
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private IEnumerable<DataRow> FindParentsOf(DataRow person)
        {
            int id = (int) person["id"];
            var ids = relations.AsEnumerable().Where((row) => row["child_id"].Equals(id));
            return people.AsEnumerable().Where((row) => ids.Any((r) => r["child_id"].Equals(row["id"])));
        }

        private IEnumerable<DataRow> FindChildOf(DataRow person)
        {
            int id = (int) person["id"];
            var ids = relations.AsEnumerable().Where((row) => row["parent_id"].Equals(id));
            return people.AsEnumerable().Where((row) => ids.Any((r) => r["child_id"].Equals(row["id"])));
        }

        private static IEnumerable<DataRow> JoinEnums(IEnumerable<IEnumerable<DataRow>> enums)
        {
            IEnumerable<DataRow> result = enums.First();
            return enums.Aggregate(result, (current, item) => current.Union(item));
        }

        private IEnumerable<DataRow> FindRelatedOfSet(Func<DataRow, IEnumerable<DataRow>> relation,
            IEnumerable<DataRow> persons)
        {
            var related = new IEnumerable<DataRow>[persons.Count()];
            int i = 0;
            var enumerator = persons.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var current = enumerator.Current;
                related[i] = relation(current);
                i++;
            }
            return JoinEnums(related);
        }

        private static IEnumerable<DataRow> Males(IEnumerable<DataRow> persons)
        {
            return persons.Where((row) => row["gender"].Equals("m"));
        }

        private static IEnumerable<DataRow> Females(IEnumerable<DataRow> persons)
        {
            return persons.Where((row) => row["gender"].Equals("f"));
        }
    }
}
