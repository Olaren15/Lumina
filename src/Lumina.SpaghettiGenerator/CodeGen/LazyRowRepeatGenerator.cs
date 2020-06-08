using System.Text;
using Lumina.Data.Structs.Excel;

namespace Lumina.SpaghettiGenerator.CodeGen
{
    public class LazyRowRepeatGenerator : BaseShitGenerator
    {
        private readonly ExcelColumnDefinition[] _cols;
        public uint Count { get; set; }
        
        public LazyRowRepeatGenerator( string typeName, string fieldName, uint columnId, uint count, ExcelColumnDefinition[] cols ) : base( typeName, fieldName, columnId )
        {
            _cols = cols;
            Count = count;
        }

        public override void WriteFields( StringBuilder sb )
        {
            sb.AppendLine( $"public LazyRow< {TypeName} >[] {FieldName};" );
        }

        public override void WriteReaders( StringBuilder sb )
        {
            // todo: this won't be the same column for each but assuming the types are the same and the repeat is correct, this will just work™
            var type = Program.ExcelTypeToManaged( _cols[ ColumnId ].Type );

            if( _cols[ ColumnId ].IsBoolType )
            {
                sb.AppendLine( $"// generator error: the definition for this repeat ({FieldName}) has an invalid type for a LazyRow" );
                return;
            }
            
            sb.AppendLine( $"for( var i = 0; i < {Count}; i++ )" );
            sb.AppendLine( $"    {FieldName}[ i ] = new LazyRow< {TypeName} >( lumina, parser.ReadColumn< {type} >( {ColumnId} + i ) );" );
        }
    }
}