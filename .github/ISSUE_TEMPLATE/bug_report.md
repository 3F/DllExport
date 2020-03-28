---
name: Bug Report
about: Found error or incorrect behavior.
labels: bug
---

Steps to reproduce:

. . .

* `DllExport -version`: 
* Used Visual Studio / MSBuild / ...: 

Information from *`Data`* tab or log data: 

. . . [{attachment if log}](https://help.github.com/articles/file-attachments-on-issues-and-pull-requests/)


Demo Project files / Samples / etc.:

. . .


-----------

*(Remove this section after reading. Click `Preview` tab for convenience)*

## ! Important

1. **Try to isolate** problem via very simple code. Any project-sample would be really useful for quick inspection!
2. Please use *Detailed* or *Diagnostic* level if you've plan to attach log:
    * When build: `msbuild [your.sln] /t:Rebuild /m:4 /v:diag > build.log`
        * For VS IDE you can try *Tools* > *Options* > *Project and Solutions* > *Build and Run* > *MSBuild project build verbosity* = [ Diagnostic ]
    * When configuring: `DllExport [args if used] > cfg.log`

3. Make sure that you have all rights to publish any of your (or company) data (attached src, log, etc). Responsibility is solely on you.
4. **Do not** put inside message any very long text data ( ~10 Kb+ time for attachments ). Means only as file (text-based, or zip, etc). Because of notifications through email. It's really ... [100K+](https://github.com/3F/DllExport/issues/71)
5. **Try to use** only [GitHub for your attachments and screenshots](https://help.github.com/articles/file-attachments-on-issues-and-pull-requests/) instead of other places. It's free, it's enough.
6. Please try to use [**basic formatting**](https://help.github.com/articles/creating-and-highlighting-code-blocks/) to avoid code dancing 🕺 across the page.
