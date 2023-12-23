# cisnerof - Windows anti-forensics made easy

This simple and lightweight utility will help you eliminate your *sus* logs left in your computor.

Once executed, it will automatically erase:

* Connected Device Platform ActivitiesCache.db
* AppCompat Install Logs
* Program Crash Dumps
* Explorer Icon Caches
* Program Compatibility Assistant Logs
* All prefetch, superfetch files
* ~~Quick launch links~~ (disabled)
* RecentFileCache.bcf
* Recent Items (Recent/*.lnk, Jumplist)
* Recycle Bin
* SRUM (SRUDB.dat)
* ~~Start menu links~~ (disabled)
* AmCache (delete ALL entries under 'InventoryApplication*' keys)
* AppCompatCache, ShimCache
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

## Documents (Mostly Korean)

* http://www.forensic-artifacts.com/windows-forensics
* https://yum-history.tistory.com/286
* https://aawjej.tistory.com/197
* https://m.blog.naver.com/ksil_/222704269742

## Related repositories

* https://github.com/LordMike/OffregLib
* https://github.com/PaulNorman01/Forensia/blob/main/src/forensia/Source.cpp
