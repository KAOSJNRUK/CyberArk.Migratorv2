param(
    [Parameter(Mandatory=$true)]
    [string]$RootPath
)

$ErrorActionPreference = "Stop"

# Path to MainWindow.xaml
$xamlPath = Join-Path $RootPath "src\App\MainWindow.xaml"
if (-not (Test-Path $xamlPath)) { throw "MainWindow.xaml not found at $xamlPath" }

# Backup original
Copy-Item $xamlPath "$xamlPath.bak" -Force
Write-Host "[INFO] Backed up $xamlPath to $xamlPath.bak"

# Read content
$content = Get-Content $xamlPath -Raw

# New CSV Flow section with restored TextBoxes
$csvFlow = @"
                <!-- CSV Flow -->
                <GroupBox Header="CSV Flow">
                    <StackPanel>
                        <Button Content="Create sample CSV" Click="CreateSampleCsv_Click" Margin="0,0,0,5"/>
                        <Button Content="Select CSV" Click="SelectExistingCsv_Click" Margin="0,0,0,5"/>
                        <Button Content="Encrypt CSV" Click="EncryptSelectedCsv_Click" Margin="0,0,0,5"/>
                        
                        <TextBox Name="txtSelectedCsv" IsReadOnly="True" Margin="0,0,0,5" />
                        <TextBox Name="txtEncryptedPath" IsReadOnly="True" Margin="0,0,0,5" />
                        
                        <StackPanel Orientation="Horizontal" Margin="0,0,0,5">
                            <TextBlock Text="Batch Size:" VerticalAlignment="Center" Margin="0,0,5,0"/>
                            <TextBox Name="txtBatchSize" Width="60" Text="50"/>
                        </StackPanel>
                        <StackPanel Orientation="Horizontal" Margin="0,0,0,5">
                            <TextBlock Text="Degree of Parallelism:" VerticalAlignment="Center" Margin="0,0,5,0"/>
                            <TextBox Name="txtDop" Width="60" Text="4"/>
                        </StackPanel>

                        <Button Content="Run Import" Click="RunImportPCloud_Click"/>
                    </StackPanel>
                </GroupBox>
"@

# Replace the old CSV Flow block
$newContent = $content -replace '(?s)<!-- CSV Flow -->.*?</GroupBox>', $csvFlow

# Write back
Set-Content -Path $xamlPath -Value $newContent -Encoding UTF8
Write-Host "[OK] Restored TextBoxes in CSV Flow section of MainWindow.xaml"

# Optional: clean bin/obj for a fresh build
Get-ChildItem -Path $RootPath -Include bin,obj -Recurse -Force | Remove-Item -Recurse -Force -ErrorAction SilentlyContinue
Write-Host "[OK] Cleaned bin/obj folders. Ready to rebuild."
