# Raccoons Builds

### Editor tools

##### Menu items

- Set Hub Folder - apply the folder for the versioned builds.
- Copy Future Build Name - if Hub Folder is valid, it will search for the newest build in the folder and copy generated build name for the future with incremented BuildNumber - "ProductName_Major.Minor.Patch-BuildNumber+1" to the clipboard. For example - if the latest build is called KillOccupiers_0.2.3-3, KillOccupiers_0.2.3-4 will be copied to the clipboard. Also, the path to Hub Folder becomes the default when building and opening Save-dialog.
- Copy Current Build Version - if Hub Folder is valid, it will search for the newest build in the folder and copy its version to the clipboard.