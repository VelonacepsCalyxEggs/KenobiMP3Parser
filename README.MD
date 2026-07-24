# KenobiMp3Parser

A library I made for my Website's backend to parse MP3 files, because solutions like TagLib#, did not provide the functionality of hashing the audio stream and iterating over the metadata
at the same time in one stream.


## Features & Limitations
It is lightweight, albeit a bit unfinished and lacks some obscure features, I wrote it in a few weeks because I was interested in how the MP3 format was 
made, learned a bunch of cursed stuff. Currently does not parse all possible Tags, but has support for essential ones. 
Utilizes Span<byte> and stackalloc in some places to be fast and reduce strain on the GC.

## CLI Tool & Installation

I have also supplied an example CLI tool to go with it, because I felt it would be a great thing to have, since I made it a separate library.
It should run on almost any OS, I have attached binaries in Releases, but you may also compile it yourself, you just need .NET 10 and a bit of knowledge of how to use VS/Rider to launch/publish code.

## Contributing

If you want to contribute, please do, I am certainly not an expert in the deep parts of C# like Memory management, and there are things that I want to improve around the script, that I didn't yet get my hands on.
There are comments lying around that I feel should describe my line of thought while I was writing it.
