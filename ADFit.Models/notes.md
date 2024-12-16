

Auto generate LocalIds
?? Enum to string serialization? 

Validate for Content Types allowed for each node.

Work on enums... They are a mess...

NestedExpand is not discontinued.

# Errors
- Invalid JSON
- This JSON doesn't comply with Atlassian Document format. You can get the latest JSON schema here.

# Marks
### Marks are supported by:
- Text
- Media (border, link)

### Combinations with other marks
backgroundColor, code, em, link, strike, strong, subsup, underline marks applies to text nodes.
- The `backgroundColor` mark cannot be combined with `code` mark
- The `textColor` cannot be combined with `code` and `link`
	- `textColor` and `backgroundColor` can share the same attribute classes.

- `code` can ONLY be combined with `link`

# Nodes
## Top Nodes
### Blockquotes
- Blockquotes `Content` can only contain one of the following: `Paragraph` | `BulletList` | `OrderedList` | `CodeBlock` | `MediaGroup` | `MediaSingle`

## Child Nodes
### ListItem
- It is child node of only `bulletlist` and `orderedlist`.
- ListItem `Content` can only contain one of the following: `Paragraph` | `BulletList` | `OrderedList` | `CodeBlock` | `MediaSingle`
### Media
- It is child node of only `mediaGroup` and `mediaSingle`.
- `width` and `height` are must attributes for `mediaSingle` node.
### NestedExpand
- Can only be present inside TableCell or TableHeader

## Inline Nodes
### Text
### Date
- Date doesn't support any marks.