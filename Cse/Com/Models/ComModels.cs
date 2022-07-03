using System;
using System.Collections.Generic;

namespace CSELibrary.Com.Models {
    public class PaginationFilterDto
    {
        public List<SelectBoxDto> rowsPerPageOptions { get; set; } = new List<SelectBoxDto>() {
           new SelectBoxDto(){ label = "10", value = 10, },
           new SelectBoxDto(){ label = "25", value = 25, },
           new SelectBoxDto(){ label = "50", value = 50, },
           new SelectBoxDto(){ label = "100", value = 100, },
       };
        public int rowsPerPage { get; set; } = 10;
        public int page { get; set; } = 1;
        public int count { get; set; } = 0;

        public int createStartIndex() {
            return (this.page - 1) * this.rowsPerPage;
        }
    }

    public class SelectBoxDto
    {
        public bool flg { get; set; }
        public string label { get; set; }
        public int value { get; set; }
    }
    public class DateFromToDto
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
    public class OrderDto
    {
        public string name { get; set; }
        public bool isDesc { get; set; }
    }

    public class FileDto
    {
        public byte[] Binary { get; set; }
        public string DataType { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string Extention { get; set; }
        public string BinaryStr { get; set; }
    }
}
