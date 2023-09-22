# Cash Flow - Changelog
[GoBack](../README.md)  

## v6.4.x
- fix: Standard deviation on analysis reports
- chores: Upgrade angular version to v14
- fix: Avoid clearing the relatedValue preventing raise duplicated focus event

## v6.3.197
- Refactor the monthly target chart to show details of the income data on each month
- Upgrade packages (backend and frontend) version to the latest patchs
- Review tooltip size on chart so it doesnt break on small screens
- Use inputMode for the valueEntry input to properly resolve the virtual keyboard
- Adjust on the applications yield chart to distribute gains from account into months that it had negative gains
- Review some properties on manifest to publish the pwa on app stores

## v6.3.187
- Review the instructions files to keep track of history change logs
- Review the release workflow to define preRelease properly based on the branch name
- Upgrade dotnet to 6.0 version

## v6.3.178
- Rename api to monthlyResult as the previously name analytics was raising
- Fix analytics screen sizes for print
- Implement light and dark modes based on the browser settings
- Review the action workflow to create a tag and realease
