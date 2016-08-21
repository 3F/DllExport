#  Modification of binary assemblies. Format and specification:
#      
#  https://github.com/3F/DllExport/issues/2#issuecomment-231593744
#  
#      Offset(h) 00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F
#  
#      000005B0                 00 C4 7B 01 00 00 00 2F 00 12 05       .Ä{..../...
#      000005C0  00 00 02 00 00 00 00 00 00 00 00 00 00 00 26 00  ..............&.
#      000005D0  20 02 00 00 00 00 00 00 00 44 33 46 30 30 46 46   ........D3F00FF   <<<<
#      000005E0  31 37 37 30 44 45 44 39 37 38 45 43 37 37 34 42  1770DED978EC774B   <<<<...
#              
#      - - - -            
#      byte-seq via chars: 
#      + Identifier        = [32]bytes
#      + size of buffer    = [ 4]bytes (range: 0000 - FFF9; reserved: FFFA - FFFF)
#      + buffer of n size
#      - - - -
#      v1.2: 01F4 - allocated buffer size
#  

$Identifier = 'D3F00FF1770DED978EC774BA389F2DC9';

Function defNS([string]$dll, [string]$namespace)
{
    Write-Host "defNS: ($dll); ($namespace)"

    if([String]::IsNullOrWhiteSpace($dll) -Or ![System.IO.File]::Exists($dll)) {
        throw New-Object System.IO.FileNotFoundException("The DllExport assembly for modifications was not found.");
    }

    if([String]::IsNullOrWhiteSpace($namespace)) {
        #$namespace = "System.Runtime.InteropServices";
        throw New-Object System.ArgumentException("The namespace cannot be null or empty.");
    }

    $origin = _postfixToOrigin($dll);
    if(![System.IO.File]::Exists($origin)) {
        [System.IO.File]::Copy($dll, $origin);
    }
    else {
        [System.IO.File]::Copy($origin, $dll, $TRUE);
    }

    $enc   = [System.Text.Encoding]::UTF8;
    $ident = $enc.GetBytes($Identifier);
    
    $data = [System.IO.File]::ReadAllBytes($dll);
    if($data.Length -lt $ident.Length) {
        throw New-Object System.IO.FileNotFoundException("Incorrect size of library.");
    }

    $lpos = -1;
    for($i = 0; $i -lt $data.Length; ++$i)
    {
        $lpos = $i;
        for($j = 0; $j -lt $ident.Length; ++$j) {
            if($data[$i + $j] -ne $ident[$j]) {
                $lpos = -1;
                break;
            }
        }
        if($lpos -ne -1) {
            break;
        }
    }

    if($lpos -eq -1) {
        throw New-Object System.IO.FileNotFoundException("Incorrect library.");
    }

    # ~
    binmod $lpos $dll $enc $namespace
}

Function binmod([int]$lpos, [string]$dll, $enc, [string]$namespace)
{
    $stream = $null;
    try
    {
        $stream = New-Object System.IO.FileStream($dll, [System.IO.FileMode]::Open, [System.IO.FileAccess]::ReadWrite);
        $stream.Seek($lpos + $ident.Length, [System.IO.SeekOrigin]::Begin);

        $bsize = New-Object byte[] 4;
        $stream.Read($bsize, 0, 4);

        $buffer = sysrange(
            [System.Convert]::ToUInt16($enc.GetString($bsize), 16)
        );

        $nsBytes = $enc.GetBytes($namespace);
        $fullseq = $ident.Length + $bsize.Length + $buffer;

        # beginning of miracles

        $nsb = New-Object 'System.Collections.Generic.List[byte]'
        $nsb.AddRange($nsBytes);
        $nsb.AddRange([System.Linq.Enumerable]::Repeat([byte]0x00, $fullseq - $nsBytes.Length));

        $stream.Seek($lpos, [System.IO.SeekOrigin]::Begin);
        $stream.Write($nsb.ToArray(), 0, $nsb.Count);

        $fs = [System.IO.File]::Create($(_postfixToUpdated($dll)));
        $fs.write($nsBytes, 0, $nsBytes.Length);
        $fs.Dispose();

        Write-Host 'The DllExport Library has been modified !'
        Write-Host "namespace: $namespace"
        Write-Host 'Details here: https://github.com/3F/DllExport/issues/2'
    }
    finally {
        if($stream -ne $null) {
            $stream.Dispose();
        }
    }

}

Function sysrange([uint16]$val)
{
    # reserved: FFFA - FFFF
    
    if($val -lt 0xFFFA) {
        return $val;
    }

    $reserved = 0xFFFF - $val;
    
    # ...
    throw New-Object System.NotImplementedException("The reserved combination is not yet implemented or not supported: " + $reserved);
}

Function _postfixToUpdated([string]$dll)
{
    return "$dll.updated";
}

Function _postfixToOrigin([string]$dll)
{
    return "$dll.origin";
}