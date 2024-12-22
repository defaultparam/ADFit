# TODO
- Validations, to see if the json schema is correct for ADF?
	- Make models richer? use of const and better null handling

- Support for following:
	- media?

- Add comments

Improvements:
- Allow reading html from files or links.
- Implement better logging Info, Warnings and Errors.
- Use result pattern for error handling? (make it generic in order to log out objects with messages)
- Implement Test cases.
- Change how the converter API is called. 
	- Instead of `HtmlAdfConverter.ConvertToHtml`... Use `new AdfDoc().ConvertToHtml()` or plan something based on the use case.