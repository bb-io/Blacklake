# Blackbird.io Blacklake

Blackbird is the new automation backbone for the language technology industry. Blackbird provides enterprise-scale automation and orchestration with a simple no-code/low-code platform. Blackbird enables ambitious organizations to identify, vet and automate as many processes as possible. Not just localization workflows, but any business and IT process. This repository represents an application that is deployable on Blackbird and usable inside the workflow editor. 

## Introduction

<!-- begin docs -->

> 💡 Blacklake is live in early access today for select Blackbird customers. If you're interested in participating in the next round of users then shoot us a message!

Blacklake is a modern linguistic data platform designed for today’s continuous, multi-system content environment. It’s built for teams who create, translate, and manage dynamic, multi-channel content.

## Before setting up

Before you can connect you need to make sure that:

- You have received Blacklake credentials

## Connecting

1. Navigate to Apps, and identify the Blacklake app. You can use search to find it.
2. Click _Add Connection_.
3. Name your connection for future reference e.g. 'My Blacklake connection'.
4. Fill in the URL to your Blacklake server.
5. Fill in the accompanying Key.
6. Click _Connect_.

![1763034376733](image/README/1763034376733.png)

## Actions

#### Core features

- **Prepare Content** takes any Blackbird interoperable compatible file and converts it into a bilingual file that can be used for further translation/processing steps. See the [Blacklake key concepts](https://docs.blackbird.io/blacklake/key-concepts/) for more information on this Action.
- **Store Content** takes any Blackbird interoperable compatible file and stores it into Blacklake. There are two distinct scenarios in which this Action is used:
  - Sending a translated file (first example below) will record the translation in Blacklake. It will store all the metadata and source units next to their translations. Applying the Lake after this action will repopulate the right units so that you significantly safe on translation cost.
  - Sending a monolingual file (second example) will commit the file without any translation metadata, but will make changes to the target of translations if they exist. This enables you to synchronize any edits made outside of the translation process into Blacklake and enable "in-context editing".

#### Metadata
- **Update boolean metadata**, **Update number metadata** and **Update text metadata** allow you to store additional metadata about content in Blacklake. This can later be used for advanced queries.

## Examples

#### Translation
![1764777794069](image/README/1764777794069.png)

The example above shows how Blacklake can be leveraged in any translation process. Before translation, "Prepare Content" is used to diff and pre-translate any content that already exists in the Lake. It also adds any relevant linguistic assets that subsequent steps can use.

After translation, and after the translation is uploaded to the source system, Blacklake uses the output of the upload action to commit the changes to the Lake using the "Store Content" action. This will make the changes and all the metadata that was generated during the translation process appear in Blacklake and available for future application.

It's important to use the output of the upload Action as this Action will further enhance the uploaded file with extra context of the source system. F.e. what is the new link of the uploaded content?

#### In context editing
![1764777830834](image/README/1764777830834.png)

The example above shows how changes in the source system to **any content** can be synchronized back into Blacklake. Implementation of this Bird will depend on the structure of the CMS. In Contentful any change to any language will trigger all languages to be republished. In a system like Hubspot this is more language dependent and thus the shape of the Bird will be different.

## Feedback

Do you want to use this app or do you have feedback on our implementation? Reach out to us using the [established channels](https://www.blackbird.io/) or create an issue.

<!-- end docs -->
