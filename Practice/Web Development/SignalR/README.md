# Real-Time Web Communication with SignalR

This repository is an enterprise-grade ASP.NET Core SignalR reference implementation paired with a React 19 + TypeScript + Tailwind CSS frontend. It is designed as a curriculum-grade technical reference that engineering trainees can follow to reproduce the complete architecture from scratch.

---

## 1. System Overview & Architectural Flow

### 1.1 Traditional Polling vs. Persistent Duplex

In a conventional HTTP interaction, the client initiates every data exchange. The server cannot proactively push data to the client without a prior request. Applications requiring real-time updates compensate with one of three polling strategies:

- **Short polling**: The client dispatches repeated HTTP requests at fixed intervals. Most requests return no new data, wasting bandwidth and server resources.
- **Long polling**: The client holds an HTTP request open; the server responds only when new data is available, then closes the connection immediately. The client reopens the connection to wait again. This simulates push behavior but still incurs HTTP overhead per message.
- **Server-Sent Events (SSE)**: A unidirectional server-to-client stream over a persistent HTTP connection. The client cannot send data back through this channel.

SignalR eliminates polling by maintaining a persistent, full-duplex connection. The server can push data to any connected client at any time, with no prior client request required.

### 1.2 Transport Negotiation

When a client connects to a SignalR hub, the following negotiation sequence occurs:

1. The client sends an HTTP POST to the server's negotiate endpoint (`/hubs/chat/negotiate`).
2. The server responds with a connection token and the list of transports it supports.
3. The client attempts to establish a connection using the best available transport, in order of preference:
   - **WebSockets** (preferred): Full-duplex TCP connection. Lowest latency and overhead. Bidirectional with a single persistent socket.
   - **Server-Sent Events**: Fallback if WebSockets are blocked by a proxy or firewall. Unidirectional (server to client only).
   - **Long Polling**: Ultimate fallback supported universally. Highest latency.

SignalR abstracts this negotiation entirely. Developers write hub methods and client event handlers; the transport layer is selected automatically.

### 1.3 Component Topology

```
Browser (React)
    |
    |  HTTP/WebSocket (relative path: /hubs/chat)
    v
Vite Dev Server (localhost:5173)
    |
    |  Reverse proxy (ws: true, secure: false)
    v
ASP.NET Core Kestrel (localhost:7145)
    |
    +-- /hubs/chat  -->  ChatHub (Hub<IChatClient>)
    |                       |-- OnConnectedAsync / OnDisconnectedAsync
    |                       |-- SendMessage      --> Clients.All.ReceiveMessage
    |                       |-- JoinGroup        --> Groups.AddToGroupAsync
    |                       |-- LeaveGroup       --> Groups.RemoveFromGroupAsync
    |                       |-- SendMessageToGroup --> Clients.Group.ReceiveGroupMessage
    |
    +-- /api/message -->  MessageController (IHubContext<ChatHub, IChatClient>)
                            |-- POST /api/message              --> Clients.All.ReceiveMessage
                            +-- POST /api/message/group/{name} --> Clients.Group.ReceiveGroupMessage
```

The Vite proxy is critical in development: it allows the frontend at `localhost:5173` to reach the backend at `localhost:7145` via relative paths (`/hubs/chat`), bypassing browser CORS and local SSL certificate rejection on WebSocket upgrades.

---

## 2. Prerequisites & Environment Setup

| Tool | Minimum Version | Purpose |
|---|---|---|
| .NET SDK | 8.0 | Build and run the ASP.NET Core Web API |
| Node.js | 18.0 | Build and run the React frontend |
| npm | 9.0 (bundled with Node.js) | Package management for the frontend |

Verify installations:

```bash
dotnet --version
node --version
npm --version
```

---

## 3. Project Structure

```
SignalR/                              # ASP.NET Core project root
|-- Contracts/
|   `-- IChatClient.cs                # Strongly-typed client RPC interface
|-- Controllers/
|   `-- MessageController.cs          # REST trigger via IHubContext
|-- Hubs/
|   `-- ChatHub.cs                    # Hub<IChatClient> implementation
|-- Models/
|   `-- MessageDto.cs                 # Immutable DTO for REST payloads
|-- Properties/
|   `-- launchSettings.json           # Kestrel ports (HTTPS 7145, HTTP 5180)
|-- Program.cs                        # Hosting, CORS, SignalR, controllers
|-- SignalR.csproj
|-- README.md
`-- ClientApp/                        # React application
    |-- components.json               # shadcn/ui configuration
    |-- package.json
    |-- postcss.config.js
    |-- tailwind.config.js            # Semantic color tokens + animate plugin
    |-- tsconfig.json                 # Strict TS, "@/*" path alias
    |-- vite.config.ts                # Alias + dev proxy to the backend
    `-- src/
        |-- main.tsx                  # Entry point (StrictMode)
        |-- index.css                 # Tailwind layers + theme CSS variables
        |-- App.tsx                   # State, persistence, channel routing
        |-- types/chat.ts             # Domain models, constants, validation
        |-- lib/
        |   `-- utils.ts              # Canonical shadcn cn utility
        |-- utils/
        |   |-- cn.ts                 # Backward-compatible re-export
        |   `-- format.ts             # Time, day label, initials, avatar tint
        |-- hooks/
        |   `-- useSignalR.ts         # Connection lifecycle and hub calls
        `-- components/
            |-- UsernameField.tsx     # Validated display-name input
            |-- OnboardingModal.tsx   # Blocking first-run dialog
            |-- ProfileModal.tsx      # Edit display name
            |-- ui/                   # shadcn/ui primitives (generated)
            |   |-- avatar.tsx
            |   |-- badge.tsx
            |   |-- button.tsx
            |   |-- dialog.tsx
            |   |-- input.tsx
            |   |-- label.tsx
            |   |-- scroll-area.tsx
            |   |-- separator.tsx
            |   |-- sheet.tsx
            |   |-- textarea.tsx
            |   `-- tooltip.tsx
            `-- chat/
                |-- ChatLayout.tsx    # Shell: h-dvh containment, no double scrollbars
                |-- ChatSidebar.tsx   # Channels, online members, profile footer
                |-- ChatHeader.tsx    # Channel title, status indicator, tooltip
                |-- MessageList.tsx   # ScrollArea feed, auto-scroll bottom anchor
                |-- MessageItem.tsx   # One message row (avatar, timestamp, bubble)
                |-- ChatInput.tsx     # Textarea composer, Enter/Shift+Enter handlers
                `-- ConnectionDot.tsx # Status dot with tooltip
```

---

## 4. Step-by-Step Server Implementation

### 4.1 Contracts/IChatClient.cs

```csharp
namespace SignalR.Contracts;

public interface IChatClient
{
    Task ReceiveMessage(string user, string message);
    Task ReceiveGroupMessage(string groupName, string user, string message);
}
```

**Why this interface exists**: SignalR generates a strongly-typed proxy implementing `IChatClient` at runtime. By inheriting from `Hub<IChatClient>`, the `Clients` property exposes this interface instead of `dynamic`. The compiler enforces method signatures, eliminating magic strings such as `SendAsync("ReceiveMessage", ...)` and catching parameter mismatches at build time rather than at runtime.

- `ReceiveMessage`: Called on all connected clients to deliver a global broadcast.
- `ReceiveGroupMessage`: Called only on clients that have joined a specific named group (channel), carrying the group name as a routing discriminator for the client to filter into the correct message stream.

### 4.2 Models/MessageDto.cs

```csharp
namespace SignalR.Models;

public sealed record MessageDto(string User, string Message);
```

The `record` type provides immutable value semantics with built-in equality. `sealed` prevents inheritance and enables compiler optimizations. This DTO is deserialized from the JSON body of REST API requests.

### 4.3 Hubs/ChatHub.cs

```csharp
using Microsoft.AspNetCore.SignalR;
using SignalR.Contracts;

namespace SignalR.Hubs;

public class ChatHub(ILogger<ChatHub> logger) : Hub<IChatClient>
{
    public override async Task OnConnectedAsync()
    {
        logger.LogInformation("Client connected: {ConnectionId}", Context.ConnectionId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (exception == null)
            logger.LogInformation("Client disconnected: {ConnectionId}", Context.ConnectionId);
        else
            logger.LogWarning(exception, "Client disconnected with error: {ConnectionId}", Context.ConnectionId);

        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(string user, string message, CancellationToken cancellationToken)
    {
        await Clients.All.ReceiveMessage(user, message);
    }

    public async Task JoinGroup(string groupName, CancellationToken cancellationToken)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName, cancellationToken);
    }

    public async Task LeaveGroup(string groupName, CancellationToken cancellationToken)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName, cancellationToken);
    }

    public async Task SendMessageToGroup(string groupName, string user, string message, CancellationToken cancellationToken)
    {
        await Clients.Group(groupName).ReceiveGroupMessage(groupName, user, message);
    }
}
```

**Key design decisions**:

- **Primary constructor injection** (`ILogger<ChatHub> logger`): The C# 12 primary constructor pattern passes the logger directly without a backing field assignment, reducing boilerplate.
- **`Hub<IChatClient>`**: The generic hub base class wires the `Clients` property to a generated proxy that implements `IChatClient`. `Clients.All.ReceiveMessage(...)` compiles to a strongly-typed invocation; the method name is derived from the interface, not a string literal.
- **`OnDisconnectedAsync` conditional logging**: A clean disconnect (exception is null) is informational. A disconnect caused by a timeout or network error carries a non-null exception and is logged as a warning, preserving the stack trace in the structured log sink.
- **`CancellationToken` on Hub methods**: SignalR passes a `CancellationToken` that is cancelled when the calling client disconnects mid-invocation. Accepting it allows long-running hub operations to be cancelled promptly.

### 4.4 Controllers/MessageController.cs

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalR.Contracts;
using SignalR.Hubs;
using SignalR.Models;

namespace SignalR.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessageController(IHubContext<ChatHub, IChatClient> hubContext) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> BroadcastMessage(
        [FromBody] MessageDto payload,
        CancellationToken cancellationToken)
    {
        await hubContext.Clients.All.ReceiveMessage(payload.User, payload.Message);
        return Ok();
    }

    [HttpPost("group/{groupName}")]
    public async Task<IActionResult> BroadcastToGroup(
        string groupName,
        [FromBody] MessageDto payload,
        CancellationToken cancellationToken)
    {
        await hubContext.Clients.Group(groupName).ReceiveGroupMessage(groupName, payload.User, payload.Message);
        return Ok();
    }
}
```

**Why `IHubContext<ChatHub, IChatClient>` instead of `IHubContext<ChatHub>`**: The generic overload `IHubContext<THub, TClient>` exposes a strongly-typed `Clients` property implementing `TClient`. This preserves compile-time safety for `ReceiveMessage` and `ReceiveGroupMessage` invocations even from outside the hub's execution context. The non-generic `IHubContext<THub>` would require `SendAsync` with magic strings.

This controller demonstrates the external trigger pattern: any server-side component — a background service, a webhook handler, or a REST endpoint — can push real-time events to connected clients without those clients ever calling this controller directly.

### 4.5 Program.cs

```csharp
using SignalR.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowClientApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "https://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowClientApp");
app.MapControllers();
app.MapHub<ChatHub>("/hubs/chat");

app.Run();
```

**CORS constraint**: SignalR requires `AllowCredentials()` because WebSocket and SSE upgrade requests carry credentials. ASP.NET Core and the browser both prohibit combining `AllowCredentials()` with `AllowAnyOrigin()` (wildcard). The Vite dev server origins are declared explicitly. In production, replace these with the deployed frontend domain.

**Middleware order is significant**: `UseCors` must appear after `UseRouting` (implicit in `WebApplication`) but before `MapControllers` and `MapHub`. Placing it after the route handlers will cause CORS headers to be absent from hub responses.

---

## 5. Step-by-Step Client Implementation

The client is a React 19 + TypeScript application styled with Tailwind CSS 3 and shadcn/ui primitives. All SignalR behavior lives in one hook (`useSignalR`); the UI components are presentational and receive data through props.

### 5.1 Package Installation and shadcn/ui Setup

Run these commands from the `ClientApp` directory.

```bash
# SignalR client
npm install @microsoft/signalr

# Tailwind CSS 3 toolchain (generates tailwind.config.js and postcss.config.js)
npm install -D tailwindcss@3 postcss autoprefixer
npx tailwindcss init -p

# Runtime dependencies and icon package used by shadcn/ui components
npm install clsx tailwind-merge@2 class-variance-authority lucide-react tailwindcss-animate
```

Initialize shadcn/ui and add the required primitives:

```bash
npx shadcn@latest init
npx shadcn@latest add avatar button textarea scroll-area separator tooltip badge
npx shadcn@latest add dialog sheet input label
npm install lucide-react
```

Configure the `@/*` path alias in two places:

```jsonc
// tsconfig.json (inside compilerOptions)
"paths": { "@/*": ["./src/*"] }
```

```typescript
// vite.config.ts
import { fileURLToPath, URL } from 'node:url'

export default defineConfig({
  plugins: [react()],
  resolve: {
    alias: { '@': fileURLToPath(new URL('./src', import.meta.url)) },
  },
  // ...server.proxy shown in section 5.2
})
```

Create `src/lib/utils.ts` containing the standard class merger utility:

```typescript
import { clsx, type ClassValue } from 'clsx';
import { twMerge } from 'tailwind-merge';

export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs));
}
```

Configure `components.json` so the shadcn CLI knows the project layout and points `utils` at `@/lib/utils`:

```json
{
  "$schema": "https://ui.shadcn.com/schema.json",
  "style": "new-york",
  "rsc": false,
  "tsx": true,
  "tailwind": {
    "config": "tailwind.config.js",
    "css": "src/index.css",
    "baseColor": "zinc",
    "cssVariables": true,
    "prefix": ""
  },
  "aliases": {
    "components": "@/components",
    "utils": "@/lib/utils",
    "ui": "@/components/ui",
    "lib": "@/lib",
    "hooks": "@/hooks"
  },
  "iconLibrary": "lucide"
}
```

The command writes each primitive into `src/components/ui/` and installs the matching `@radix-ui/*` packages.

**Note on animations**: The generated animation classes (`animate-in`, `fade-in-0`, `slide-in-from-left`) come from the `tailwindcss-animate` plugin. It must be registered in `tailwind.config.js` (section 5.3), otherwise dialogs, the mobile drawer and tooltips appear without transitions.

**Note on dialog customization**: One local modification was made to the generated `ui/dialog.tsx`: `DialogContent` accepts an optional `hideClose` prop that omits the corner close button. The onboarding dialog uses it because it must not be dismissible. Re-running `shadcn add dialog --overwrite` removes this change.

### 5.2 ClientApp/vite.config.ts

```typescript
server: {
  port: 5173,
  proxy: {
    '/hubs': { target: 'https://localhost:7145', changeOrigin: true, secure: false, ws: true },
    '/api':  { target: 'https://localhost:7145', changeOrigin: true, secure: false },
  },
},
```

**Why the proxy is required**: in development the React app is served from `http://localhost:5173` and the backend from `https://localhost:7145`. A direct browser connection would be cross-origin and would also hit the self-signed Kestrel development certificate. The proxy makes `/hubs/chat` and `/api/*` same-origin from the browser's perspective. `ws: true` forwards WebSocket upgrades; `secure: false` skips certificate validation on the proxy-to-backend leg only.

### 5.3 Theme: Tailwind Tokens and CSS Variables

Colors are not hard-coded in components. Components use semantic utilities (`bg-background`, `bg-muted`, `text-muted-foreground`, `bg-primary`, `border`), which resolve to CSS variables defined in `src/index.css`:

```css
@layer base {
  :root {
    --background: 0 0% 100%;
    --foreground: 240 10% 3.9%;
    --primary: 240 5.9% 10%;
    --primary-foreground: 0 0% 98%;
    --muted: 240 4.8% 95.9%;
    --muted-foreground: 240 3.8% 46.1%;
    --border: 240 5.9% 90%;
    --radius: 0.5rem;
    /* remaining shadcn tokens omitted for brevity */
  }
  html, body, #app { height: 100%; margin: 0; padding: 0; }
}
```

`tailwind.config.js` maps each variable to a color name and registers the animation plugin:

```javascript
import animate from 'tailwindcss-animate';

export default {
  content: ['./index.html', './src/**/*.{js,ts,jsx,tsx}'],
  theme: {
    extend: {
      colors: {
        background: 'hsl(var(--background))',
        foreground: 'hsl(var(--foreground))',
        primary: { DEFAULT: 'hsl(var(--primary))', foreground: 'hsl(var(--primary-foreground))' },
        muted: { DEFAULT: 'hsl(var(--muted))', foreground: 'hsl(var(--muted-foreground))' },
        // border, input, ring, accent, secondary, destructive, popover, card follow the same pattern
      },
    },
  },
  plugins: [animate],
};
```

`html`, `body` and `#app` are set to `height: 100%` so the chat shell can fill the viewport exactly (section 5.6). Changing the visual theme means editing the variables in `index.css` only.

### 5.4 ClientApp/src/types/chat.ts (Key Excerpts)

```typescript
export type ConnectionStatus = 'Connected' | 'Reconnecting' | 'Disconnected';

export interface ChatMessage {
  readonly id: string;
  readonly user: string;
  readonly message: string;
  readonly channel: string;
  readonly timestamp: string;
  readonly isRestApi?: boolean;
}

export interface Channel {
  readonly id: string;
  readonly name: string;
  readonly description: string;
  readonly unreadCount?: number;
}

export const DEFAULT_CHANNELS: readonly Channel[] = [
  { id: 'general',       name: 'general',       description: 'General discussion', unreadCount: 0 },
  { id: 'engineering',   name: 'engineering',   description: 'Technical architecture and code reviews', unreadCount: 0 },
  { id: 'announcements', name: 'announcements', description: 'Official enterprise bulletins', unreadCount: 0 },
] as const;

export const STORAGE_KEYS = {
  USERNAME:       'signalr_enterprise_user',
  ACTIVE_CHANNEL: 'signalr_enterprise_active_channel',
} as const;

export function validateUsername(username: string): { isValid: boolean; error?: string } {
  const trimmed = username.trim();
  if (!trimmed)            return { isValid: false, error: 'Username is required.' };
  if (trimmed.length < 2)  return { isValid: false, error: 'Username must be at least 2 characters.' };
  if (trimmed.length > 25) return { isValid: false, error: 'Username cannot exceed 25 characters.' };
  if (!/^[a-zA-Z0-9_\-]+( [a-zA-Z0-9_\-]+)*$/.test(trimmed))
    return { isValid: false, error: 'Alphanumeric characters, underscores, hyphens, and spaces only.' };
  return { isValid: true };
}
```

Centralizing domain models in a single `types/chat.ts` file makes the contract visible to all components and the hook simultaneously. `readonly` modifiers enforce immutability in TypeScript: components cannot mutate received messages, they must derive new state through React state setters.

### 5.5 ClientApp/src/hooks/useSignalR.ts (Key Sections)

```typescript
export const useSignalR = (urlOrOptions?: string | UseSignalROptions): UseSignalRReturn => {
  const targetUrl = options.url || '/hubs/chat';

  const connectionRef = useRef<signalR.HubConnection | null>(null);
  const subscribedGroupsRef = useRef<Set<string>>(new Set([defaultChannel]));

  useEffect(() => {
    let isCancelled = false;                          // StrictMode safety flag

    const connection = new signalR.HubConnectionBuilder()
      .withUrl(targetUrl)
      .withAutomaticReconnect([0, 2000, 5000, 10000, 30000])
      .configureLogging(signalR.LogLevel.Information)
      .build();

    connectionRef.current = connection;

    connection.on('ReceiveMessage', handleReceiveMessage);
    connection.on('ReceiveGroupMessage', handleReceiveGroupMessage);

    connection.onreconnecting(() => setConnectionStatus('Reconnecting'));

    connection.onreconnected(async () => {
      setConnectionStatus('Connected');
      // Automatically rejoin all subscribed groups after reconnection
      for (const group of subscribedGroupsRef.current) {
        await connection.invoke('JoinGroup', group);
      }
    });

    const startConnection = async () => {
      try {
        await connection.start();
        if (isCancelled) {
          await connection.stop();  // Discard if StrictMode already unmounted
          return;
        }
        setConnectionStatus('Connected');
        // Join initial groups immediately upon connection
        for (const group of subscribedGroupsRef.current) {
          await connection.invoke('JoinGroup', group);
        }
      } catch (err) {
        if (!isCancelled) setConnectionStatus('Disconnected');
      }
    };

    startConnection();

    return () => {
      isCancelled = true;
      connection.off('ReceiveMessage', handleReceiveMessage);
      connection.off('ReceiveGroupMessage', handleReceiveGroupMessage);
      connection.stop();
    };
  }, [targetUrl]);
};
```

**Critical design decisions**:

- **`useRef` for the connection object**: The connection is stored in a ref rather than state. This avoids triggering re-renders when the connection object is set, and ensures the cleanup function always has access to the same connection instance across Strict Mode cycles.
- **`isCancelled` flag**: React 18 Strict Mode mounts, unmounts, and immediately remounts every component in development. This means `useEffect` runs twice. Without the `isCancelled` flag, the rapid unmount-remount cycle would start a new connection, then the cleanup from the first mount would stop it, then the second mount's connection would try to invoke groups on a stopped connection. The flag ensures that if the component was unmounted before `connection.start()` resolved, the new connection is immediately stopped and state is never updated.
- **`withAutomaticReconnect([0, 2000, 5000, 10000, 30000])`**: Custom backoff intervals define retry delay in milliseconds. The first retry is immediate (0ms), then exponential: 2s, 5s, 10s, 30s. After these five attempts are exhausted, SignalR stops retrying and fires `onclose`.
- **Group resubscription on reconnect**: The `onreconnected` callback reads the current contents of `subscribedGroupsRef` and re-invokes `JoinGroup` for each. Because the server's in-memory group membership is lost when the connection drops, the client must explicitly re-subscribe.

### 5.6 Component Roles and State Binding

State ownership is deliberately narrow. `App.tsx` owns application state and talks to the hook; everything under `components/chat/` is presentational.

| Component | Role | Inputs (state binding) |
|---|---|---|
| `App.tsx` | Owns the current user, active channel, unread counts and the profile dialog flag. Calls `useSignalR`, persists to `localStorage`, routes sends by channel. | Hook output |
| `ChatLayout` | Layout shell enforcing `h-dvh` and `overflow-hidden`. Fixed sidebar on `md` and above, a `Sheet` drawer below it. | channels, active channel, messages, connection status, callbacks |
| `ChatSidebar` | Channel switcher with unread badges, online members presence list, and profile footer. | channels, `activeChannelId`, username, status |
| `ChatHeader` | Channel title, description, presence indicator, and connection dot with tooltip. | active channel, status |
| `MessageList` | Feed powered by shadcn `ScrollArea`. Groups consecutive messages, inserts day dividers, auto-scrolls to bottom anchor. | messages for the active channel, current user |
| `MessageItem` | One row: avatar, sender, time, text. | message, `isOwn`, `showHeader` |
| `ChatInput` | Composer using shadcn `Textarea`. Enter submits, Shift+Enter adds newline; disabled unless connected. | channel name, `disabled`, `onSend` |
| `ConnectionDot` | Status indicator with tooltip and screen-reader label. | status |
| `OnboardingModal` / `ProfileModal` | Dialogs for choosing and editing the display name. | open flag, callbacks |

**Connection state.** `ConnectionDot` renders one of three states: a green dot (`Connected`, `bg-emerald-500`), an amber dot with `animate-pulse` (`Reconnecting...`, `bg-amber-500`), and a gray dot (`Disconnected`, `bg-zinc-400`). It never produces an intrusive banner. The label is exposed through a tooltip on hover and through `aria-label` for assistive technology.

**Viewport and scrolling.** The root of `ChatLayout` is `h-dvh` (with `h-screen` fallback) with `overflow-hidden`, and each flex child that must shrink carries `min-h-0`. Only the message feed scrolls via shadcn `ScrollArea`, preventing outer double scrollbars across the viewport.

**Auto-scroll.** `MessageList` utilizes a dedicated bottom anchor ref. It tracks whether the reader is within 120 px of the bottom. A new message smoothly scrolls the feed to the bottom anchor when the reader is near the bottom or when the local user sent it. If the reader scrolled up to inspect message history, position is retained without abrupt scrolling interruptions. Switching channels remounts `MessageList` and starts at the bottom.

**Sender distinction.** Own messages are right-aligned with `bg-primary text-primary-foreground` fill; messages from peers are left-aligned with `bg-muted text-foreground` fill and an initials avatar whose tint is derived from the name. Consecutive messages from the same sender within five minutes share one header. Text uses `whitespace-pre-wrap`, `break-words` and `overflow-wrap: anywhere`, ensuring long unbroken strings wrap inside the bubble. Times are formatted with `Intl.DateTimeFormat` as `10:42 AM`.

**Composer rules** (`ChatInput`):

- Uses shadcn `Textarea` with auto-submit on `Enter` and line break on `Shift + Enter`.
- The text is trimmed before dispatch and empty submissions are prevented.
- The textarea and send button (icon `SendHorizontal`) are disabled while the status is not `Connected`.
- The field is cleared immediately on submit, focus is automatically retained afterwards, and if the send rejects, the draft is restored unless the user has already typed a new message.

**Dialogs.** `OnboardingModal` is a Radix `Dialog` with Escape, outside-click and the close button disabled, so a valid name is required. `ProfileModal` can be dismissed and enables Save only for a valid name that differs from the current one. Each form is a child component mounted only while its dialog is open, so draft state resets without an effect.

### 5.7 ClientApp/src/App.tsx (Key Sections)

```typescript
// Channel routing: #general uses the global broadcast, other channels use groups.
const handleSendMessage = useCallback(
  async (text: string) => {
    if (isOnboardingRequired) return;
    if (activeChannelId === DEFAULT_CHANNEL_ID) {
      await sendMessage(currentUser, text);
    } else {
      await sendMessageToGroup(activeChannelId, currentUser, text);
    }
  },
  [isOnboardingRequired, activeChannelId, currentUser, sendMessage, sendMessageToGroup]
);

// Channel switching: leave the previous group, join the next, reset its unread count.
const handleSelectChannel = useCallback(async (channelId: string) => {
  const next = normalizeChannelId(channelId);
  if (next === activeChannelId) return;
  await leaveGroup(activeChannelId);
  await joinGroup(next);
  setActiveChannelId(next);
  setUnreadCounts((prev) => ({ ...prev, [next]: 0 }));
}, [activeChannelId, leaveGroup, joinGroup]);

// Only messages for the active channel reach the feed.
const activeMessages = useMemo(
  () => messages.filter((msg) => normalizeChannelId(msg.channel) === activeChannelId),
  [messages, activeChannelId]
);
```

`handleSendMessage` does not catch errors. It rejects so that `ChatInput` can restore the draft; the hook also records the error message in its own state.

The `#general` channel uses `SendMessage` (`Clients.All`) and the other channels use `SendMessageToGroup` (`Clients.Group`). Messages that arrive through the REST endpoints take the same event path as hub messages, so they render as ordinary messages.

---

## 6. Testing and Verification Runbook

### 6.1 Starting the Application

**Terminal 1 — Backend**:
```bash
# From the project root
dotnet run --launch-profile https
```
The backend starts and listens on `https://localhost:7145` (HTTPS) and `http://localhost:5180` (HTTP). The SignalR hub is mapped at `/hubs/chat`. Swagger UI is available at `https://localhost:7145/swagger`.

**Terminal 2 — Frontend**:
```bash
cd ClientApp
npm install    # First run only
npm run dev
```
Vite starts on `http://localhost:5173` with the proxy active.

### 6.2 Verification Steps

#### Step 1: Onboarding Gate
Open `http://localhost:5173`. The onboarding modal appears immediately. The "Join Workspace" button is disabled until a valid username is entered (2–25 characters, alphanumeric with spaces/hyphens/underscores). Confirm that submitting an empty value or a single character shows an error message and does not dismiss the modal.

#### Step 2: Connection Establishment
After entering a valid username, the sidebar connection badge should transition to **Connected** (green) within one second. If it remains **Disconnected**, check that the backend is running and the Vite proxy target matches the backend port.

#### Step 3: Client-to-Server Broadcast (Multi-Tab Test)
Open `http://localhost:5173` in two additional browser tabs. In Tab 1, send a message. Confirm the message appears in Tabs 2 and 3 with no page refresh, proving `SendMessage → Clients.All.ReceiveMessage` is working end-to-end.

#### Step 4: REST API Push Test
```bash
curl -X POST "https://localhost:7145/api/message" ^
  -H "Content-Type: application/json" ^
  -d "{\"user\": \"System\", \"message\": \"Global broadcast from REST API\"}" ^
  --insecure
```
On Linux/macOS:
```bash
curl -X POST "https://localhost:7145/api/message" \
  -H "Content-Type: application/json" \
  -d '{"user": "System", "message": "Global broadcast from REST API"}' \
  --insecure
```
All connected tabs should display the message in real time.

#### Step 5: Channel Isolation Test
In Tab 1, click `#engineering` in the sidebar. The UI switches to the engineering channel. In Tab 2, remain on `#general`. Send a message from Tab 1's engineering channel. Confirm the message appears only in Tab 1 and not in Tab 2, proving group isolation through `SendMessageToGroup → Clients.Group(groupName)`.

#### Step 6: REST API Group Push Test
```bash
curl -X POST "https://localhost:7145/api/message/group/engineering" ^
  -H "Content-Type: application/json" ^
  -d "{\"user\": \"System\", \"message\": \"Engineering channel alert\"}" ^
  --insecure
```
Only tabs currently viewing the `#engineering` channel should receive this message.

#### Step 7: Unread Count Verification
With two tabs open on different channels, send a message from one tab. Confirm the inactive channel in the other tab shows an unread count badge in the sidebar.

#### Step 8: Two-Session Message Exchange
Use two separate browser sessions so each has its own `localStorage` and therefore its own display name. A normal window plus a private window works; two ordinary tabs share one stored name.

1. Start the backend and the frontend (section 6.1).
2. Window A: open `http://localhost:5173`, enter the name `alice`, continue.
3. Window B (private window): open the same URL, enter `bob`, continue.
4. In both windows confirm the green dot and the `Connected` label in the header.
5. In window A, type `hello`, press Enter. Expected: the message appears right-aligned in window A as `You`, and left-aligned in window B with the sender `alice`, an avatar and the time. The input in window A is empty and still focused.
6. Press Shift+Enter in the input: nothing is sent. Submit empty or whitespace-only text: nothing is sent.
7. Send a 200-character string without spaces. Expected: it wraps inside the bubble and no horizontal scrollbar appears.
8. Stop the backend. Expected within a few seconds: the dot turns amber and pulses (`Reconnecting...`), then gray (`Disconnected`) after the retry schedule is exhausted. The input and send button are disabled. Restart the backend and reload; the dot returns to green.
9. Send enough messages to overflow the viewport. Expected: only the message list scrolls, the page itself has no scrollbar, and new messages keep the feed at the bottom. Scroll up and have the other window send a message: the feed does not jump.

---

## 7. Production Considerations & Common Pitfalls

### 7.1 CORS and AllowCredentials

**The constraint**: SignalR's WebSocket handshake and SSE stream require the browser to include credentials. ASP.NET Core enforces that `AllowCredentials()` cannot be combined with a wildcard `AllowAnyOrigin()` policy. Attempting this combination throws an `InvalidOperationException` at startup.

**The fix**: Always declare origins explicitly:
```csharp
policy.WithOrigins("https://app.yourdomain.com")
      .AllowAnyHeader()
      .AllowAnyMethod()
      .AllowCredentials();
```

In production, read the allowed origin from `appsettings.json` or environment variables rather than hardcoding it.

### 7.2 React Strict Mode Double-Connection

React 18 Strict Mode (enabled in `main.tsx` via `<React.StrictMode>`) intentionally mounts, unmounts, and remounts every component in development. This causes `useEffect` to execute twice, which would ordinarily create two concurrent SignalR connections — resulting in duplicate message delivery.

The mitigation implemented in `useSignalR.ts`:

1. **`useRef` for the connection instance**: A ref does not trigger re-renders and persists across the unmount/remount cycle.
2. **`isCancelled` boolean**: Set to `true` in the cleanup function. If `connection.start()` resolves after the cleanup has run (i.e., the component was unmounted before the connection established), the code checks `isCancelled`, stops the connection immediately, and skips all state updates.
3. **Explicit `connection.stop()` in cleanup**: The cleanup function unconditionally stops the connection. The second mount creates a fresh `HubConnectionBuilder` and starts a new, clean connection.

This pattern ensures exactly one live connection per mounted `App` component in both development (Strict Mode) and production.

### 7.3 Hardcoded URLs in Development

The Vite proxy target (`https://localhost:7145`) is hardcoded in `vite.config.ts`. This is acceptable in development because port 7145 is defined by `launchSettings.json` and is consistent across developer machines.

For production, the frontend should read the backend URL from an environment variable:

```typescript
// vite.config.ts
const backendUrl = process.env.VITE_BACKEND_URL || 'https://localhost:7145';
```

```typescript
// In application code, use import.meta.env:
const HUB_URL = import.meta.env.VITE_HUB_URL || '/hubs/chat';
```

### 7.4 Horizontal Scaling

SignalR connections are bound in-memory to the server process that established them. In a horizontally scaled deployment with a load balancer distributing traffic across multiple instances:

- Client A's WebSocket connection is pinned to Instance 1.
- Client B's WebSocket connection is pinned to Instance 2.
- A broadcast invoked on Instance 1 reaches only the clients connected to Instance 1. Client B on Instance 2 never receives it.

Two architectural solutions address this:

**Redis Backplane**: All server instances subscribe to a shared Redis pub/sub channel. When Instance 1 broadcasts, it publishes to Redis. All instances (including Instance 2) receive the publication and forward it to their locally connected clients.

```csharp
builder.Services.AddSignalR()
    .AddStackExchangeRedis("redis-connection-string");
```

**Azure SignalR Service**: A fully managed service where clients negotiate with the application server but are redirected to the Azure service for connection management. The application server maintains a small pool of multiplexed connections to Azure and uses them to broadcast. This offloads all WebSocket management to Azure, enabling the application servers to scale independently without a backplane configuration.

```csharp
builder.Services.AddSignalR()
    .AddAzureSignalR("azure-signalr-connection-string");
```

Azure SignalR Service is the recommended approach for production deployments on Azure, as it also provides connection limits beyond what a single Kestrel process can handle.

### 7.5 Authentication and Authorization

The current implementation has no authentication. For production:

- Protect hub methods with `[Authorize]` attributes.
- Pass JWT tokens to the SignalR connection:
  ```typescript
  .withUrl('/hubs/chat', {
    accessTokenFactory: () => localStorage.getItem('access_token') ?? ''
  })
  ```
- Configure the ASP.NET Core authentication middleware before SignalR in `Program.cs`.

---

## 8. WebSocket vs. SignalR: Feature Comparison

| Feature | Raw WebSocket | SignalR |
|---|---|---|
| Type | Protocol (RFC 6455) | Library / Framework |
| Communication | Full-duplex bidirectional | Full-duplex bidirectional + broadcast + grouping |
| Connection management | Manual | Automatic reconnect, lifecycle callbacks |
| Transport fallback | None (WebSocket only) | WebSocket → SSE → Long Polling |
| Message format | Binary or UTF-8 text frames | JSON or MessagePack |
| Group/room support | None (manual implementation) | Built-in Groups API |
| Strongly-typed RPC | None | Hub<T> with compile-time interface checking |
| Scale-out backplane | Manual implementation | Redis or Azure SignalR Service |
