### How to reproduce

... 
    
#### Version, configuration, commands:

Versions: 

* `-version`:
* `-build-info`:
* Full command: 
* Environment ( VS, MSBuild, ...): 

Configuration:

...

#### Project files, samples:

...

### log

...




-----------

Please note *(Remove this section after reading. Click `Preview` tab for convenience)*

1. **Try to use** only [GitHub for your attachments and screenshots](https://help.github.com/articles/file-attachments-on-issues-and-pull-requests/) instead of other places. It's free, it's enough.
2. **Try to isolate** problem via very simple code. Any project-samples would be really useful for quick inspection!
3. **Do not** put inside message any very long text data ( ~ 10 Kb+ time for attachments ). Only as file (text-based, or zip, etc). Because we're receiving this notification through email, so it's really ...
4. Please try to use [basic formatting for your code examples](https://help.github.com/articles/creating-and-highlighting-code-blocks/), to avoid code dancing across the page.
5. You can attach any data to help understand your problem, However, **do not forget about copyrights, license etc.** of your (or company) program, because this code will be available for 3rd party side, thus please check that you have required rights !
6. Log data. To set **detailed** or **diagnostics** level:
    * For VS IDE: `Tools` - `Options` - `Project and Solutions` - `Build and Run` - MSBuild project build verbosity 
    * For msbuild.exe: `/v:diag`:

```
msbuild <sln> /t:Rebuild /m:4 /v:diag > build.log
```
