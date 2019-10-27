---
name: Bug Report
about: Found error or incorrect behavior.
labels: bug
---

How to reproduce:

... 


* `-version`:
* `-build-info`:
* Full command to Manager: 
* Project type: 
* Environment ( VS, MSBuild, ...): 

Used configuration:

...

Optional Project files / Samples:

...

log:

* [{attachment}](https://help.github.com/articles/file-attachments-on-issues-and-pull-requests/) ...



-----------

*(Remove this section after reading. Click `Preview` tab for convenience)*

## ! Important

1. **Try to isolate** problem via very simple code. Any project-sample would be really useful for quick inspection!
2. Log data. To set **detailed** or **diagnostics** level:
    * For VS IDE: `Tools` - `Options` - `Project and Solutions` - `Build and Run` - *MSBuild project build verbosity.* 
    * For msbuild tools: `/v:diag`:

```
msbuild <your.sln> /t:Rebuild /m:4 /v:diag > build.log
```
3. Make sure that you have all rights to publish any of your (or company) data (attached src, log, etc). Responsibility is solely on you.
4. **Do not** put inside message any very long text data ( ~10 Kb+ time for attachments ). Means only as file (text-based, or zip, etc). Because of notifications through email. It's really ... [100K+](https://github.com/3F/DllExport/issues/71)
5. **Try to use** only [GitHub for your attachments and screenshots](https://help.github.com/articles/file-attachments-on-issues-and-pull-requests/) instead of other places. It's free, it's enough.
6. Please try to use [**basic formatting** for your code examples](https://help.github.com/articles/creating-and-highlighting-code-blocks/), to avoid code dancing 🕺 across the page.
