class Solution:
    def racecar(self, target: int) -> int:
        dq = deque([(1, 0)])
        visited = set()
        moves = -1
        while dq:
            moves += 1
            L = len(dq)
            for _ in range(L):
                speed, pos = dq.popleft()
                if pos == target:
                    return moves
                new_val = (speed*2, pos + speed)
                if new_val not in visited:
                    visited.add(new_val)
                    dq.append(new_val)
                new_val = (2*(speed < 0) - 1, pos)
                if new_val not in visited:
                    visited.add(new_val)
                    dq.append(new_val)
        return 0