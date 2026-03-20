# Blackbird.io Blacklake

Blackbird is the new automation backbone for the language technology industry. Blackbird provides enterprise-scale automation and orchestration with a simple no-code/low-code platform. Blackbird enables ambitious organizations to identify, vet and automate as many processes as possible. Not just localization workflows, but any business and IT process. This repository represents an application that is deployable on Blackbird and usable inside the workflow editor.

## Introduction

<!-- begin docs -->

> You can try Blacklake today. Contact us if you want to participate.

Blacklake is a modern linguistic data platform designed for today's continuous, multi-system content environment. It's built for teams who create, translate, and manage dynamic, multi-channel content.

## Before setting up

Before you can connect you need to make sure that:

- You have received Blacklake credentials

## Connecting

1. Navigate to Apps, and identify the Blacklake app. You can use search to find it.
2. Click _Add Connection_.
3. Name your connection for future reference e.g. 'My Blacklake connection'.
4. Fill in the URL to your Blacklake server.
5. Fill in your Blacklake project key.
6. Click _Connect_.

![1763034376733](image/README/1763034376733.png)

## Actions

### Content

- **Prepare Content** Prepare a file for translation by leveraging matching content from a lake and applying relevant variant assets.
    Advanced settings:
    - **Source content ID**: Specify the source content ID when your CMS stores translations without links to each other.
    - **Strategy ID**: Select a leveraging strategy configured in your Blacklake. If not set, content is only diffed.
    - **Prepare for**: Choose whether to prepare the content for translation or editing.
- **Store Content** Store translated or monolingual content in a lake for future leveraging.
    Advanced settings:
    - **Align with variant code**: If you are storing monolingual content, align it with a source variant.

> The source content ID advanced setting is only needed when your CMS stores translations without links between source and target content.

There are two distinct scenarios in which the **Store Content** Action is used:
- Sending a translated file (first example below) records the translation in Blacklake. It stores the metadata and source units next to their translations so future preparation can reuse the right units and reduce translation cost.
- Sending a monolingual file (second example) stores the file without translation metadata, but updates existing target content when available. This helps synchronize edits made outside the translation process and supports in-context editing.

### Metadata

- **Update text metadata** Update a text metadata value for a content variant.
- **Update number metadata** Update a number metadata value for a content variant.
- **Update boolean metadata** Update a boolean metadata value for a content variant.

> Metadata can be used in leveraging to perform KPI-based content usage.

For more features, examples and FAQ see [our docs](https://docs.blackbird.io/blacklake/introduction/).

## Examples

#### Translation
![1764777794069](image/README/1764777794069.png)

The example above shows how Blacklake can be used in a translation process. Before translation, "Prepare Content" is used to diff and pre-translate content that already exists in the lake. It also adds relevant variant assets that subsequent steps can use.

After translation, and after the translation is uploaded to the source system, Blacklake uses the output of the upload action to commit the changes to the lake using the "Store Content" action. This makes the changes and all metadata generated during the translation process available for future use.

It's important to use the output of the upload action because it can include additional source-system context, such as the new link of the uploaded content.

#### In context editing
![1764777830834](image/README/1764777830834.png)

The example above shows how changes in the source system to **any content** can be synchronized back into Blacklake. Implementation of this bird depends on the structure of the CMS. In Contentful, any change to any language triggers all languages to be republished. In a system like HubSpot, this is more language-dependent, so the bird shape will differ.

## Feedback

Do you want to use this app or do you have feedback on our implementation? Reach out to us using the [established channels](https://www.blackbird.io/) or create an issue.

<!-- end docs -->
