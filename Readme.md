# ByteScout XML Doc Filter command line utiltiy

Copyright (c) 2016, [ByteScout](https://bytescout.com).

Takes XML document and filters out nodes by their name

# Usage:

`Usage:  XmlDocFilter.exe /input <filename> output <filename> /filters <filters>`
`/input <filename>   Input XML file.`
`/output <filename>  Output XML file.`
`/filter <filters>   Comma-separated filter strings.`

Example:


`XmlDocFilter.exe /input:MyProject.xml output:MyProject.Filtered.xml /filters:.Internal.,Core.`
