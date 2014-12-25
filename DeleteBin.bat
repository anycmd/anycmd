@for /r . %%I in (TestResults) do if exist "%%I" rd/s/q "%%I"
@for /r . %%I in (bin) do if exist "%%I" rd/s/q "%%I"
@for /r . %%I in (obj) do if exist "%%I" rd/s/q "%%I"

del /s/q/f *.user
del /s/q/f *.suo

