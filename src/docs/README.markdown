# Docs
[Fornax]: https://github.com/ionide/Fornax
[guides]:
[posts]:
[controls]

The docs website is built using [Fornax], to contribute to the docs you can add a new (or update an existing) markdown file in the
[guides], [posts], [controls] directories.

## Guides
Guides are a general way to explain how to achieve a result or to take a user through an explanation of an existing concept inside Avalonia.FuncUI.

Guides follow the following format

```markdown
---
layout: guide
title: Some Title
author: Some Author
list-order: 1
guide-category: category
tags: my, list, of, optional, tags
---
[A internal website link]: <relativeurl>/<guide>.html
[An external url]: https://an-external.website.com/
[Basic Template]: guides/Basic-Template.html

<YOUR MARKDOWN CONTENT>
You can checkout [Basic Template]
```
Possible values are
- layout -> guide
    (always use guide)
- title -> any string that describes the title
- list-order -> an integer used to determine the order of appearance in the left menu on the website, this is an ascending integer
- guide-category -> a GuideCategory UC, you can use the following strings
    - beginner
    - intermediate
    - advanced
    - uncategorized
- author (optional) -> any string that denominates the author of the guide
- tags (optional) -> comma separated strings

Please put Links on the top of the document, also use relative URL's when linking documents that exist inside the Avalonia.FuncUI Website.
See the `[Basic Template]: guides/Basic-Template.html` example, you can refer to other documents in the [guides] directory


## Posts
Posts follow a similar structure of guides
```markdown
---
layout: post
title: Some Title
author: Some Author
published: 2020-02-20
tags: my, list, of, optional, tags
---
[A internal website link]: <relativeurl>/<guide>.html
[An external url]: https://an-external.website.com/
[Basic Template]: guides/Basic-Template.html

<YOUR MARKDOWN CONTENT>
You can checkout [Basic Template]
```
- layout -> post
    (always use post)
- title -> any string that describes the title
- published (optional) -> a string in the following format YYYY-MM-DD
- author (optional) -> any string that denominates the author of the guide
- tags (optional) -> comma separated strings

In the same way you have to put Links at the top and use relative URLs when linking content inside the Avalonia.FuncUI Website


### Preview while Developing
Fornax offers a watch command that allows you to preview the website as you develop it's contents

- install the fornax-cli tool
- change directory into the docs sources
- fornax watch
- open your browser on localhost:8080 (for further information visit the [Fornax] repository)

> If you want to live reload your changes (instead of an F5 on each change) you can select set to false the `siteContent.Add({ disableLiveRefresh = true })` setting inside `loaders/guideloader.fsx` and `loaders/postloader.fsx`

> to prevent issues with links locally please change the follwing line `baseUrl = "/Avalonia.FuncUI/"` to `baseUrl = "/"` while in development in `loaders/globalloader.fsx`

```
dotnet tool install fornax -g
cd src/docs
fornax watch
```


### Build Docs
To update the website, we have to build the docs sources, we use a fake script for this

> Pleas note that if you are only adding/updating content (example: adding a .md file) you don't need to include the updated docs in your pull request, if you added a different layout, or modified an existing page (example: generators/index.fsx) please notify in the pull request so it can also be verified by the maintainers

- install fornax cli tool
- change directory into the docs sources
- ./fake run build.fsx -t CopyDocs
- Docs have been put into the docs directory in the root of the repository

```
dotnet tool install fornax -g
cd src/docs
./fake run build.fsx -t CopyDocs
```
now you can push or PR the built website, after a few minutes of the PR (or change) being merged into master the website will reflect these changes


# ***Note***
`.markdown` files
To prevent the [Fornax] engine from auto-processing files we use the `.markdown` file extension for files that we want to load individually on certain pages (like `index.`markdown inside the `index.fsx` file)