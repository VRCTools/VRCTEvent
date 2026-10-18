# Contributing to VRCTools

Thanks for your interest in improving VRCTools! This guide applies to all VRCTools repositories - the packages
(such as [VRCTWorldUtils](https://github.com/VRCTools/VRCTWorldUtils),
[VRCTEvent](https://github.com/VRCTools/VRCTEvent) and [VRCTCommon](https://github.com/VRCTools/VRCTCommon)) as well as
the [website & documentation](https://github.com/VRCTools/website).

> [!IMPORTANT]
> **We do not accept AI-generated code or assets in our packages.** Using AI as writing assistance for documentation
> is fine. See [AI-generated contributions](#ai-generated-contributions) below.

## Ways to contribute

- **Ask and answer questions** on our [Discord server](https://discord.gg/FGrt9wv5CF).
- **Report bugs** you run into while building your world.
- **Suggest features** or improvements to existing components.
- **Improve the documentation** at [vrctools.io/docs](https://vrctools.io/docs/) - every page has an
  *Edit this page* link.
- **Submit code** fixes and features to the packages.

## Reporting bugs

Before opening an issue, please search the existing issues of the repository - your problem may already be known or
fixed in a newer version.

A good bug report contains:

- the **package and version** you are using (see *Manage Project* in Creator Companion, or the package's
  `package.json`),
- your **Unity and VRChat SDK versions**,
- **steps to reproduce** the problem, ideally in a small test scene,
- **what you expected** to happen and **what happened instead**,
- relevant **console output** (errors and warnings) and screenshots of the involved inspectors.

If you are not sure whether something is a bug, ask on [Discord](https://discord.gg/FGrt9wv5CF) first.

> [!NOTE]
> Please report security issues privately - send a direct message to a maintainer on Discord instead of opening a
> public issue.

## Suggesting features

Open an issue describing the **problem you are trying to solve** in your world, not just the solution you have in mind.
Examples of how you would use the feature help a lot. For larger ideas, starting a conversation on Discord first is
often the fastest way to find out whether it fits the package.

## Contributing code

### Before you start

For anything beyond a small, obvious fix, please **open an issue or talk to us on Discord before you start
working**. This avoids duplicate work and makes sure your change fits the direction of the package - we would hate
for you to spend time on a pull request we can't accept.

### Setting up

Every package repository is a complete Unity project with the package itself located in `Packages/io.vrctools.<name>`.

1. Fork the repository and clone your fork.
2. Open the project folder with **Creator Companion** (or ALCOM / vrc-get) to resolve the VRChat SDK and other
   dependencies, then open it in the **Unity version** listed in `ProjectSettings/ProjectVersion.txt`.
3. Create a branch for your change: `git checkout -b fix/slider-mapping`.

### Making changes

- **Keep pull requests focused.** One fix or feature per pull request is much easier to review than several at once.
- **Follow the existing code style.** Match the surrounding code: two space indentation, explicit `this.`
  qualification, public Udon events prefixed with an underscore (`_Toggle`) and XML documentation comments for public
  members. New source files carry the same Apache 2.0 license header as existing ones.
- **Commit Unity `.meta` files.** Every new asset, script or folder inside the package needs its `.meta` file -
  without it, references break for everyone else.
- **Don't change unrelated files.** Revert incidental changes Unity makes to scenes, project settings or packages
  you didn't intend to touch.
- **Keep it compatible.** Avoid renaming public fields, events and functions - existing worlds reference them. If a
  breaking change is unavoidable, call it out in the pull request.
- **Leave version numbers alone.** Releases (including the version in `package.json`) are handled by the
  maintainers.

### Testing

UdonSharp code only runs inside VRChat (or ClientSim), so please test your change before submitting it:

- Test in **play mode with ClientSim**, using the package's sample scene or a small test scene.
- For anything **networked**, test with multiple clients via **Build & Test** (set *Number of Clients* to 2 or more).
- For **persisted** values, rejoin to verify that data is restored.
- Check the console for new errors or warnings.

Describe what you tested in the pull request.

### Submitting a pull request

1. Push your branch and open a pull request against `main`.
2. Explain **what** the change does and **why**, and link the related issue (`Fixes #123`).
3. Include screenshots or a short clip for visual or inspector changes.
4. Be patient - VRCTools is maintained in our free time. We review pull requests as soon as we can and may ask for
   changes before merging.

## Contributing to the documentation

The documentation is part of the website repository, [VRCTools/website](https://github.com/VRCTools/website), and is
written in Markdown. Small fixes can be made directly on GitHub via the *Edit this page* link at the bottom of every
page. For larger changes, see the
[documentation writing guide](https://vrctools.io/docs/project/documentation/), which covers local previews, page
structure and writing style.

When a code change in a package affects how something is used, a matching pull request to the documentation is very
welcome.

## AI-generated contributions

**We do not accept contributions to our packages that were created in whole or in part using generative AI tools** -
for example large language model based coding assistants, chat bots or code generators. This applies to code, assets,
tests and pull request descriptions.

**Documentation is the exception:** you may use AI tools as writing assistance for documentation - both inside the
code (comments and XML documentation) and in the [website repository](https://github.com/VRCTools/website). You
remain responsible for what you submit, so please review AI-assisted text carefully and make sure it is accurate.

By submitting a pull request to one of our packages, you confirm that its code and assets are your own work. Pull requests
that we find to contain AI-generated code or assets will be closed without review.

Bug reports, questions and feature requests are always welcome - please write them in your own words.

## License

VRCTools is released under the [Apache License, Version 2.0](https://www.apache.org/licenses/LICENSE-2.0). By
submitting a contribution, you agree that it is licensed under the same terms as the repository you are contributing
to.
