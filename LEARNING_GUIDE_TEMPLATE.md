# Learning Guide Template & Pattern

This document outlines the structure, language patterns, and best practices for creating interactive learning guide pages in the BlazorMock application.

## Overview

Each learning guide step follows a consistent, friendly pattern designed to help developers learn by doing. The guides combine:

- Conversational, encouraging language
- Complete, copy-paste ready code examples
- Detailed step-by-step breakdowns
- Live interactive demos
- Progress tracking

## File Structure

Each step requires three files:

```
Components/Pages/Examples/[Topic]/Step[N]/
├── Example.razor          # The learning guide page (tutorial content)
├── Example.razor.cs       # Code-behind for guide (progress tracking, demo logic)
└── [Feature]Example.razor # Standalone working demo component
└── [Feature]Example.razor.cs # Code-behind for standalone demo
```

## Page Structure

### 1. Header Section

```razor
@page "/[topic]-examples/step[N]"
@rendermode InteractiveServer
@inherits ExampleBase

<PageTitle>[Topic] Step [N]: [Title] - Example</PageTitle>

<div class="min-h-screen py-8 rounded-4xl bg-gray-50">
    <div class="w-full mx-auto px-4 max-w-5xl">
        <!-- Back link -->
        <a href="/[topic]-guide" class="inline-flex items-center gap-2...">
            <span>←</span> Back to [Topic] Guide
        </a>

        <!-- Title -->
        <h1 class="text-4xl font-bold text-gray-900 mb-4">
            [Emoji] Step [N]: [Title]
        </h1>

        <!-- Description -->
        <p class="text-xl text-gray-600 mb-8">
            [Friendly, conversational description of what they'll accomplish]
        </p>
```

**Language Tips:**

- Use second person ("you'll", "your")
- Be encouraging and positive
- Explain the "why" not just the "what"
- Example: "Time to make it shine!" instead of "Add styling"

### 2. What You'll Learn Section (Gradient Banner)

```razor
<div class="bg-gradient-to-r from-gray-100 to-blue-100 rounded-2xl p-5 sm:p-6 mb-6">
    <h2 class="text-xl sm:text-2xl font-semibold text-gray-800 mb-4">What You'll Learn</h2>

    <p class="text-gray-700 mb-4 text-sm sm:text-base">
        [1-2 sentences: Friendly explanation of why this step matters and what it builds on]
    </p>

    <ul class="list-disc ml-6 text-sm sm:text-base text-gray-700 space-y-2 mb-4">
        <li>[Specific skill/outcome 1]</li>
        <li>[Specific skill/outcome 2]</li>
        <li>[Specific skill/outcome 3]</li>
        <li>[Specific skill/outcome 4]</li>
    </ul>

    <div class="mt-2 pt-3 border-t border-blue-200">
        <p class="text-xs sm:text-sm text-gray-700 mb-2">
            <strong>💡 Key Concepts:</strong> [Concept 1], [Concept 2], [Concept 3]
        </p>
        <div class="flex flex-wrap items-center gap-2">
            <span class="text-xs sm:text-sm text-gray-600">→ Related tips:</span>
            <a href="/tips#[tip-id]" class="px-3 py-1 bg-blue-100 text-blue-700 rounded-full...">
                [Tip Name]
            </a>
        </div>
    </div>
</div>
```

**Language Tips:**

- Keep bullet points action-oriented
- Use everyday language: "make your grid adapt" not "implement responsive design"
- Explain benefits: "add smooth animations when you hover" not "add hover effects"

### 3. Sample Component Section (White Card)

```razor
<div class="bg-white rounded-xl p-6 border border-gray-200 mb-6">
    <h2 class="text-xl font-bold text-gray-900 mb-4">🧩 Sample Component: [Component Name]</h2>

    <p class="text-sm text-gray-600 mb-4">
        [2-3 sentences: Friendly overview of what this component does and what makes it special]
    </p>

    <!-- Full code example -->
    <div class="bg-gray-100 border border-gray-200 rounded-lg p-4 overflow-x-auto mb-4">
        <div class="text-xs text-gray-500 mb-1">Components/Pages/[Path]/[File].razor</div>
        <p class="text-xs text-gray-600 mb-2">[Brief description of this code]</p>
        <ul class="list-disc ml-4 mb-3 text-[11px] text-gray-600 space-y-1">
            <li>[Key feature/behavior 1]</li>
            <li>[Key feature/behavior 2]</li>
            <li>[Key feature/behavior 3]</li>
        </ul>
        <pre data-code-title="Razor ([FileName].razor)"
            class="code-block text-xs sm:text-sm font-mono text-gray-800 w-full max-w-full overflow-x-auto">
[Complete copy-paste ready code]
</pre>
    </div>

    <!-- If code-behind exists -->
    <div class="bg-gray-100 border border-gray-200 rounded-lg p-4 overflow-x-auto mb-4">
        <div class="text-xs text-gray-500 mb-1">Components/Pages/[Path]/[File].razor.cs</div>
        <p class="text-xs text-gray-600 mb-2">[Brief description of code-behind logic]</p>
        <ul class="list-disc ml-4 mb-3 text-[11px] text-gray-600 space-y-1">
            <li>[What it manages/handles 1]</li>
            <li>[What it manages/handles 2]</li>
        </ul>
        <pre data-code-title="C# ([FileName].razor.cs)"
            class="code-block text-xs sm:text-sm font-mono text-gray-800 w-full max-w-full overflow-x-auto">
[Complete code-behind code]
</pre>
    </div>
```

**Code Block Tips:**

- Always include complete, working code
- Use HTML entities for Razor syntax: `@@` for `@`, `&lt;` for `<`, `&gt;` for `>`
- Keep code examples realistic and functional
- Add comments in code where helpful

### 4. Step-by-Step Breakdown

```razor
    <!-- Step-by-step breakdown -->
    <h3 class="text-sm font-semibold text-gray-800 mb-2">Step-by-step breakdown</h3>
    <div class="grid gap-4">
        <div class="bg-gray-100 border border-gray-200 rounded-lg p-4">
            <div class="text-xs text-gray-500 mb-1">[FileName].razor / [FileName].razor.cs</div>
            <div class="text-xs text-gray-500 mb-1">[N]. [Action-oriented step title]</div>
            <p class="text-xs text-gray-600 mb-2">
                [1-2 sentences: Explain what this step does in simple terms]
            </p>
            <ul class="list-disc ml-4 mb-2 text-[11px] text-gray-600 space-y-1">
                <li>[Specific detail about the code]</li>
                <li>[Why this pattern/approach works]</li>
                <li>[What it accomplishes]</li>
                <li>[How it connects to other parts]</li>
            </ul>
            <pre class="code-block text-xs sm:text-sm font-mono text-gray-800 w-full max-w-full overflow-x-auto">
[Relevant code snippet for THIS step only]
</pre>
        </div>
        <!-- Repeat for each step -->
    </div>
</div>
```

**Step Breakdown Tips:**

- 8-12 steps is ideal (not too few, not overwhelming)
- Each step should be focused on ONE concept/action
- Show only the relevant code for that step
- Explain in conversational terms: "Create methods to handle Previous/Next buttons"
- Include 4-5 bullet points explaining the code
- Progress logically: setup → data → UI → interactions

### 5. Important Notes & Tips Section

```razor
<div class="bg-white rounded-xl p-6 border border-gray-200 mb-6">
    <h2 class="text-xl sm:text-2xl font-semibold text-gray-800 mb-4">📝 Important Notes & Tips</h2>

    <div class="space-y-3">
        <!-- Pro Tips (Blue) -->
        <div class="bg-blue-50 border border-blue-200 rounded-lg p-4">
            <h3 class="text-sm font-bold text-blue-900 mb-3">💡 Pro Tips</h3>
            <div class="space-y-4">
                <div>
                    <p class="text-sm font-semibold text-blue-900 mb-2">[Tip headline]</p>
                    <p class="text-xs text-blue-800">[Explanation with practical advice]</p>
                </div>
                <!-- 3-5 tips total -->
            </div>
        </div>

        <!-- Troubleshooting (Yellow) -->
        <div class="bg-yellow-50 border border-yellow-200 rounded-lg p-4">
            <h3 class="text-sm font-bold text-yellow-900 mb-2">⚠️ Troubleshooting</h3>
            <ul class="list-disc ml-6 text-sm text-yellow-900 space-y-1">
                <li><strong>[Problem]:</strong> [Solution/explanation]</li>
                <!-- 4-6 common issues -->
            </ul>
        </div>
    </div>
</div>
```

**Tips Section Guidelines:**

- Pro Tips: Best practices, optimization, advanced techniques
- Troubleshooting: Common errors students will encounter
- Be specific: reference actual code, class names, properties
- Explain the "why" behind issues

### 6. How to Do It Section (Gradient Banner)

```razor
<div class="bg-gradient-to-r from-gray-100 to-blue-100 rounded-2xl p-5 sm:p-6 mb-6">
    <h2 class="text-xl sm:text-2xl font-semibold text-gray-800 mb-4">🛠️ How to do it</h2>
    <ol class="list-decimal ml-5 space-y-2 text-gray-700 text-sm sm:text-base">
        <li>
            <strong>[Action]:</strong>
            [Clear instruction with file paths and specific names]
        </li>
        <li>
            <strong>[Action]:</strong>
            [Clear instruction]
        </li>
        <!-- 5-8 steps total -->
    </ol>
</div>
```

**How To Guidelines:**

- Keep it sequential and actionable
- Include file paths and code references
- End with "Run and test" type instructions
- Make it feel achievable

### 7. Live Demo Section

```razor
<div class="bg-black/3 rounded-2xl p-6 mb-6">
    <h2 class="text-2xl font-semibold text-gray-800 mb-4">🎬 Live Demo</h2>
    <p class="text-sm text-gray-600 mb-4">
        [Encourage them to interact with the demo and explain what to look for]
    </p>

    <div class="bg-white rounded-2xl border border-gray-200 p-6">
        @if (isLoading)
        {
            [Loading state UI]
        }
        else if (!string.IsNullOrEmpty(errorMessage))
        {
            [Error state UI]
        }
        else
        {
            [Actual working demo component]
        }
    </div>

    <p class="text-xs text-gray-600 mt-4 text-center">
        💡 [Helpful tip about what the demo demonstrates]
    </p>
</div>
```

### 8. Mark Complete Section (Green Card)

```razor
<div class="bg-white rounded-xl p-6 border-2 border-green-200 mb-6">
    <div class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
        <div>
            <h3 class="text-lg font-semibold text-gray-900 mb-1">Finished Step [N]?</h3>
            <p class="text-sm text-gray-600">[Completion criteria]</p>
        </div>
        @if (isComplete)
        {
            <div class="flex items-center gap-3">
                <span class="px-4 py-2 rounded-full bg-green-100 text-green-700 font-medium">✓ Completed</span>
                <button @onclick="ResetStep" class="px-4 py-2 border border-gray-300 rounded-lg hover:bg-gray-50 transition-colors text-sm">
                    Reset
                </button>
            </div>
        }
        else
        {
            <button @onclick="MarkComplete" class="px-6 py-3 bg-green-600 hover:bg-green-700 text-white rounded-lg font-medium transition-colors">
                Mark as Complete
            </button>
        }
    </div>
</div>
```

### 9. Navigation Section

```razor
<div class="mt-8">
    <div class="bg-black/90 rounded-2xl p-6 sm:p-8 text-white text-center">
        <p class="text-lg sm:text-xl font-semibold mb-4">[Encouraging message]! [Emoji]</p>
        <p class="text-sm text-gray-300 mb-6 max-w-2xl mx-auto">
            [Recap what they accomplished and tease what's next]
        </p>
        <div class="flex flex-col sm:flex-row items-center justify-center gap-3">
            <a href="/[topic]-guide" class="inline-flex items-center gap-2 px-6 py-3 bg-white/10 hover:bg-white/20 rounded-xl transition-colors">
                <span class="text-lg">←</span>
                <span class="text-sm sm:text-base font-semibold">Back to Guide</span>
            </a>
            <a href="/[topic]-examples/step[N-1]" class="inline-flex items-center gap-2 px-6 py-3 bg-white/10 hover:bg-white/20 rounded-xl transition-colors">
                <span class="text-lg">←</span>
                <span class="text-sm sm:text-base font-semibold">Previous: Step [N-1]</span>
            </a>
            <a href="/[topic]-examples/step[N+1]" class="inline-flex items-center gap-2 px-6 py-3 bg-blue-600 hover:bg-blue-700 rounded-xl transition-colors">
                <span class="text-sm sm:text-base font-semibold">Next: Step [N+1]</span>
                <span class="text-lg">→</span>
            </a>
        </div>
    </div>
</div>
```

## Language & Tone Guidelines

### Voice & Style

- **Friendly and conversational**: Write like you're sitting next to someone helping them learn
- **Encouraging**: Celebrate progress, acknowledge challenges
- **Clear and simple**: Avoid jargon; explain technical terms
- **Action-oriented**: Use active voice and strong verbs

### Examples of Good vs. Poor Phrasing

| ❌ Too Technical                           | ✅ Friendly & Clear                                       |
| ------------------------------------------ | --------------------------------------------------------- |
| "Implement responsive grid layout"         | "Make your grid adapt beautifully from mobile to desktop" |
| "Add CSS transitions for state changes"    | "Add smooth animations that feel professional"            |
| "Format numeric values with leading zeros" | "Make all IDs the same width by adding leading zeros"     |
| "The user will invoke the API endpoint"    | "You'll connect to the PokeAPI and grab Pokemon data"     |
| "Utilize LINQ Skip and Take operators"     | "Slice the list to show just the current page"            |

### Sentence Starters to Use

- "Let's..." - collaborative and friendly
- "You'll..." - direct and personal
- "This..." - for explanations
- "Notice how..." - for teaching moments
- "Try..." - for encouraging experimentation

### Words to Favor

- Simple: "grab", "show", "build", "make"
- Active: "connect", "fetch", "display", "update"
- Visual: "glow", "smooth", "clean", "polished"
- Encouraging: "great", "perfect", "awesome", "nice"

### Words to Avoid

- Overly technical: "instantiate", "implement", "utilize"
- Passive: "is created", "will be handled"
- Academic: "furthermore", "thus", "therefore"
- Discouraging: "simply", "just", "obviously" (implies it should be easy)

## Code Example Best Practices

### Full Code Examples

- Always provide complete, runnable code
- Include all necessary imports/directives
- Use realistic data and scenarios
- Add inline comments for complex sections
- Escape special characters properly for HTML rendering

### Step-by-Step Code Snippets

- Show ONLY the code relevant to that specific step
- Include enough context (3-5 lines before/after)
- Explain what each part does in bullet points
- Progress logically through the component's construction

### Code Comments

```csharp
// Good: Explains the "why"
totalPages = (int)Math.Ceiling(allPokemon.Count / (double)pageSize);  // Round up for partial last page

// Less helpful: Restates the "what"
totalPages = (int)Math.Ceiling(allPokemon.Count / (double)pageSize);  // Calculate total pages
```

## Responsive Design

All sections use Tailwind's responsive classes:

- Text: `text-sm sm:text-base` or `text-xs sm:text-sm`
- Padding: `p-4 sm:p-6`
- Gaps: `gap-2 sm:gap-3`
- Grids: `grid-cols-1 sm:grid-cols-2 lg:grid-cols-3`
- Flex direction: `flex-col sm:flex-row`

## Color Coding

- **Blue**: Information, tips, links
- **Green**: Success, completion
- **Yellow**: Warnings, troubleshooting
- **Red**: Errors, critical issues
- **Gray**: Secondary information, code backgrounds

## Accessibility Considerations

- Use semantic HTML (`<h1>`, `<h2>`, `<nav>`, etc.)
- Provide alt text for images
- Ensure sufficient color contrast
- Make interactive elements keyboard accessible
- Use descriptive link text

## Testing Your Guide

Before publishing, verify:

- [ ] All code examples are complete and runnable
- [ ] Step-by-step breakdown covers all major concepts
- [ ] Language is friendly and encouraging throughout
- [ ] Navigation links work correctly
- [ ] Live demo functions properly
- [ ] Progress tracking (Mark Complete) works
- [ ] Responsive design looks good on mobile
- [ ] No broken links to tips or other resources
- [ ] Code formatting is correct (proper escaping)
- [ ] Consistent emoji usage and styling

## Common Patterns

### Introducing New Concepts

```
[Concept] lets you [benefit]. Here's how it works: [simple explanation].
```

Example: "aspect-square forces every image container to be perfectly square, creating a clean grid."

### Explaining Code Decisions

```
We use [pattern/approach] because [reason]. This [benefit it provides].
```

Example: "We use Skip and Take because they let you slice the list without re-fetching data from the API. This makes pagination instant!"

### Transitioning Between Steps

```
Now that you have [what they just did], let's [what comes next].
```

Example: "Now that you have your data loading, let's add pagination so users can browse through all the Pokemon."

## File Naming Conventions

- Guide pages: `Example.razor` (always)
- Code-behind: `Example.razor.cs` (always)
- Demo components: `[Feature]Example.razor` (e.g., `PokemonCardsExample.razor`)
- Routes: `/[topic]-examples/step[N]` for guides, `/[topic]-examples/demo-[feature]` for standalone demos

## Conclusion

This template creates a consistent, engaging learning experience that:

- Reduces cognitive load with familiar structure
- Encourages hands-on learning with working examples
- Provides multiple learning modes (reading, doing, exploring)
- Tracks progress and builds confidence
- Uses language that welcomes all skill levels

Remember: The goal is not just to teach code, but to make developers feel capable and excited about building!
