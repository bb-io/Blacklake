using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Blacklake.Models;
public class Error
{
    public string Title { get; set; }
    public int Status { get; set; }
    public Dictionary<string, List<string>> Errors { get; set; }
}