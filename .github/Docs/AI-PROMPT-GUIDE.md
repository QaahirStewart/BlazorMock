# AI Assistant Prompt Templates

This guide provides standardized prompts to ensure the AI assistant reads and updates all relevant documentation files when working on the BlazorMock project.

---

## 🎯 Core Principle

**Always read before writing.** The AI should check existing documentation, understand context, and update all related files.

---

## 📋 Standard Prompt Templates

### 1. Starting a New Step

```
I'm working on Step [N] of the BlazorMock learning guide.

Before we begin:
1. Read Docs/Steps/Step[N].md
2. Read Docs/BlazorLearningGuide.md (Step [N] section)
3. Read Docs/BlazorLearningChecklist.md (Step [N] section)
4. Check if Docs/Step1-ProjectSetup.md needs updating (if applicable)

Then:
- Implement the code for Step [N]
- Update ALL documentation files that reference this step
- Ensure cross-links between steps are correct
- Mark what needs verification in the checklist
```

### 2. Updating Documentation

```
I need to update documentation for [topic/feature].

Before making changes:
1. Read Docs/README.md to understand structure
2. Read all files that mention [topic/feature]
3. Check Docs/DOCUMENTATION-STRUCTURE.md for organization
4. Review Docs/STYLE-GUIDE.md for formatting rules

Then:
- Update all affected documentation files
- Ensure consistency across all mentions
- Update cross-references
- Follow style guide conventions
```

### 3. Adding a New Feature

```
I want to add [feature name] to BlazorMock.

Before implementing:
1. Read relevant step docs in Docs/Steps/
2. Check Docs/BlazorLearningGuide.md for context
3. Review Docs/STYLE-GUIDE.md for code conventions
4. Check Docs/Typography-System.md for UI patterns (if UI change)

Then:
- Implement the feature
- Update Docs/BlazorLearningGuide.md
- Update Docs/BlazorLearningChecklist.md
- Add to relevant step docs
- Update Docs/UPDATES.md with change log
```

### 4. Refactoring Code

```
I need to refactor [component/service/feature].

Before refactoring:
1. Read all documentation that references the code being changed
2. Check Docs/STYLE-GUIDE.md for current conventions
3. Review related step docs in Docs/Steps/
4. Check Docs/IMPLEMENTATION-STATUS.md

Then:
- Refactor the code
- Update all documentation references
- Update code examples in docs
- Mark changes in Docs/UPDATES.md
```

### 5. Fixing Bugs

```
There's a bug in [location/feature].

Before fixing:
1. Read relevant documentation to understand intended behavior
2. Check Docs/Steps/ for the step that introduced this feature
3. Review Docs/STYLE-GUIDE.md for best practices

Then:
- Fix the bug
- Update documentation if behavior changed
- Add notes to Docs/UPDATES.md
- Update examples in step docs if needed
```

### 6. Adding UI Components

```
I want to add a new UI component: [ComponentName].

Before creating:
1. Read Docs/Typography-System.md
2. Read Docs/STYLE-GUIDE.md (UI & Styling section) - Note Tailwind preference for examples
3. Check existing components for patterns
4. Review Docs/Steps/ to see where it fits

Then:
- Create the component following style guide
- Use Tailwind classes in all Razor examples (not minimal HTML)
- Update Docs/BlazorLearningGuide.md if it's part of a step
- Add component pattern to Docs/STYLE-GUIDE.md if reusable
- Update relevant step documentation
```

### 7. Comprehensive Update

```
I need a comprehensive review/update of [system/feature/section].

Read in this order:
1. Docs/README.md - Overall structure
2. Docs/DOCUMENTATION-STRUCTURE.md - How files relate
3. All Docs/Steps/Step[XX].md files related to [topic]
4. Docs/BlazorLearningGuide.md - [topic] sections
5. Docs/BlazorLearningChecklist.md - [topic] items
6. Docs/STYLE-GUIDE.md - Relevant conventions
7. Any other specific docs mentioned in above files

Then:
- Make changes systematically
- Update ALL related documentation
- Ensure no broken links
- Verify cross-references
- Update Docs/UPDATES.md with summary
```

---

## 🔄 Automatic Documentation Update Checklist

When the AI makes ANY change, it should automatically:

### Always Check These Files

- [ ] `Docs/Steps/Step[XX].md` - If step-related
- [ ] `Docs/BlazorLearningGuide.md` - Update step description
- [ ] `Docs/BlazorLearningChecklist.md` - Update checklist items
- [ ] `Docs/UPDATES.md` - Log the change
- [ ] `Docs/README.md` - If structure changed

### Check If Applicable

- [ ] `Docs/Step1-ProjectSetup.md` - If setup process changed
- [ ] `Docs/Typography-System.md` - If UI/styling changed
- [ ] `Docs/STYLE-GUIDE.md` - If new patterns introduced
- [ ] `Docs/ProgressTrackingGuide.md` - If progress system changed
- [ ] `Docs/IMPLEMENTATION-STATUS.md` - If feature status changed
- [ ] `Docs/FINAL-STATUS.md` - If completion status changed

### Cross-Reference Updates

- [ ] Update "Next step" links if order changed
- [ ] Update "Previous step" links if order changed
- [ ] Update live example routes if pages changed
- [ ] Update file references if files moved/renamed

---

## 📝 File Reference Quick List

### Core Documentation Files to Always Consider

| File                              | Update When                           |
| --------------------------------- | ------------------------------------- |
| `Docs/README.md`                  | Structure changes, new major docs     |
| `Docs/STYLE-GUIDE.md`             | New patterns, conventions established |
| `Docs/DOCUMENTATION-STRUCTURE.md` | File organization changes             |
| `Docs/BlazorLearningGuide.md`     | Any step content changes              |
| `Docs/BlazorLearningChecklist.md` | Step tasks change                     |
| `Docs/Steps/Step[XX].md`          | Specific step changes                 |
| `Docs/UPDATES.md`                 | ANY change to track history           |

### Specialized Documentation Files

| File                            | Update When                   |
| ------------------------------- | ----------------------------- |
| `Docs/Step1-ProjectSetup.md`    | Setup process changes         |
| `Docs/Typography-System.md`     | UI/styling patterns change    |
| `Docs/ProgressTrackingGuide.md` | Progress system changes       |
| `Docs/ELI5.md`                  | Explanations added/updated    |
| `Docs/IMPLEMENTATION-STATUS.md` | Feature implementation status |
| `Docs/FINAL-STATUS.md`          | Project completion status     |
| `Docs/COMPLETION-SUMMARY.md`    | Summary updates               |
| `Docs/README-STATUS.md`         | README status tracking        |

---

## 🎨 Example Prompts in Action

### Example 1: Adding Step 14

```
I want to add Step 14: Authentication & Authorization.

Read first:
- Docs/Steps/Step13.md (to link from)
- Docs/BlazorLearningGuide.md (all step 13 references)
- Docs/BlazorLearningChecklist.md (where Step 14 goes)
- Docs/STYLE-GUIDE.md (documentation format)
- Docs/README.md (to add Step 14 to index)

Then create:
- Docs/Steps/Step14.md (with proper links to/from Step 13)
- Update Docs/Steps/Step13.md (add "next" link)
- Add Step 14 section to Docs/BlazorLearningGuide.md
- Add Step 14 checklist to Docs/BlazorLearningChecklist.md
- Update Docs/README.md step list
- Log in Docs/UPDATES.md
```

### Example 2: Changing Step Order

```
I need to swap Step 6 and Step 7 (Navigation should come before Models).

Read and update:
- Docs/Steps/Step05.md (update "next" link)
- Docs/Steps/Step06.md (rename to Step07, update prev/next links)
- Docs/Steps/Step07.md (rename to Step06, update prev/next links)
- Docs/Steps/Step08.md (update "previous" link)
- Docs/BlazorLearningGuide.md (swap sections 6 and 7)
- Docs/BlazorLearningChecklist.md (swap sections 6 and 7)
- Docs/README.md (update step list order)
- Docs/UPDATES.md (log the reordering)
```

### Example 3: Adding New Component Pattern

```
I've created a new modal component pattern that should be reusable.

Read and update:
- Docs/STYLE-GUIDE.md (add modal pattern to Component Patterns)
- Docs/Typography-System.md (if typography is involved)
- Relevant Docs/Steps/Step[XX].md (if introduced in a step)
- Docs/BlazorLearningGuide.md (if part of curriculum)
- Docs/UPDATES.md (log new pattern)
```

---

## 🚀 Quick Start for AI

When you (the AI) receive a request:

### Step 1: Identify Scope

- What files are affected?
- Which step(s) does this relate to?
- What documentation needs updating?

### Step 2: Read First

```
Use file_search or read_file to check:
1. All step docs mentioned in the request
2. Main guide files (Learning Guide, Checklist)
3. Style guide for conventions
4. Any cross-referenced files
```

### Step 3: Make Changes

- Update code/implementation
- Update ALL related documentation
- Ensure consistency

### Step 4: Verify Links

- Check all cross-references still work
- Verify step progression (prev/next)
- Ensure no orphaned references

### Step 5: Log Changes

- Update `Docs/UPDATES.md`
- Note what was changed and why

---

## 💡 Pro Tips for Users

### Get the AI to Read Documentation First

**Bad Prompt:**

> "Add a tooltip component"

**Good Prompt:**

> "I need a tooltip component. First, read Docs/STYLE-GUIDE.md for UI patterns and Docs/Typography-System.md for styling. Then create the component and update relevant docs."

### Ensure Comprehensive Updates

**Bad Prompt:**

> "Update Step 5"

**Good Prompt:**

> "Update Step 5. Read Docs/Steps/Step05.md, Docs/BlazorLearningGuide.md (Step 5 section), and Docs/BlazorLearningChecklist.md (Step 5 items). Make changes and update all three files plus any cross-references."

### Maintain Documentation Sync

**Always remind the AI:**

> "After making changes, update Docs/UPDATES.md and verify all cross-references in Docs/Steps/ are still correct."

---

## 📌 Template for Complex Tasks

```
Task: [Describe what you want to do]

Pre-read checklist:
□ Docs/README.md
□ Docs/STYLE-GUIDE.md
□ Docs/Steps/Step[XX].md (specify which)
□ Docs/BlazorLearningGuide.md
□ Docs/BlazorLearningChecklist.md
□ [Any other specific files]

Implementation:
1. [Step 1]
2. [Step 2]
3. [Step 3]

Post-update checklist:
□ Update Docs/BlazorLearningGuide.md
□ Update Docs/BlazorLearningChecklist.md
□ Update Docs/Steps/Step[XX].md
□ Update cross-references
□ Log in Docs/UPDATES.md
□ Verify no broken links
```

---

## 🎯 Summary

**Golden Rule:** The AI should ALWAYS read existing documentation before making changes, and ALWAYS update all related files after changes.

**Minimum Read:**

- The specific step docs
- Main guide and checklist
- Style guide

**Minimum Update:**

- The files that were changed
- Cross-references
- Update log (UPDATES.md)

**Use these templates consistently to ensure documentation stays synchronized!**

---

**Last Updated:** October 14, 2025  
**Purpose:** Ensure AI assistant maintains documentation consistency
