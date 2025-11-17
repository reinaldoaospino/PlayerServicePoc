# PlayerService - Dapr Actors POC

A simple proof of concept demonstrating Dapr Actors with .NET 9 for managing player points in a gaming/betting system.

## What This Does

- Each player is a virtual actor with their own state
- Points are stored in Redis and persist across restarts
- Dapr handles actor lifecycle, concurrency, and state management
- Everything runs in Docker containers

## Prerequisites

- Docker Desktop installed and running
- That's it! No .NET SDK, Dapr CLI, or Redis needed on your machine

## Quick Start

### 1. Clone and Navigate

```bash
cd /path/to/PlayerService
