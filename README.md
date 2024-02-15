# AntiForensic.NET :: Windows anti-forensics made easy

This simple and lightweight library and utility will help you eliminate *all* logs from your computor.

Usage of this utility vary from just simply learning various anti-forensic technique to simply remove all usage traces from the PC to evade tracing.

## Action

Once executed, it will automatically erase:

* Connected Device Platform ActivitiesCache.db
* AppCompat Install Logs
* Program Crash Dumps
* Explorer Icon Caches
* Program Compatibility Assistant Logs
* All prefetch, superfetch files
* Quick launch links (disabled by default)
* RecentFileCache.bcf
* Recent Items (Recent/*.lnk, Jumplist)
* Recycle Bin
* SRUM (SRUDB.dat)
* Start menu links (disabled by default)
* AmCache (delete ALL entries under 'InventoryApplication*' keys)
* AppCompatCache, ShimCache
* AppCompatFlags Store
* BAM (Background Activity Moderator) Logs
* Explorer Last-visited MRU
* MUI cache
* Explorer Open-save MRU
* Explorer Recent Docs
* Run(Ctrl+R) MRU
* Shellbags
* UserAssist
* Event logs
* UsnJrnl

TODO:

* [ ] Powershell command logs
* [ ] MFT log ($Logs, $Bitmap)
* [ ] Various program 'Recent Files' registry, files (mostly 'Unarchivers' such as 7-zip, Bandizip, etc.)


## Documents (Mostly Korean)

* http://www.forensic-artifacts.com/windows-forensics
* https://yum-history.tistory.com/286
* https://aawjej.tistory.com/197
* https://blog.naver.com/ksil_/222704269742

## Related repositories

* OffregLib :: https://github.com/LordMike/OffregLib
* Forensia :: https://github.com/PaulNorman01/Forensia/blob/main/src/forensia/Source.cpp
