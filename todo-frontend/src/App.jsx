import { useState, useEffect } from 'react';

// Use environment variable or fallback to local backend port
const API_URL = import.meta.env.VITE_API_URL || 'http://localhost:5035';

function App() {
  const [todos, setTodos] = useState([]);
  const [newTodo, setNewTodo] = useState('');

  // READ Records
  useEffect(() => {
    fetch(`${API_URL}/api/todos`)
      .then(res => res.json())
      .then(data => setTodos(data))
      .catch(err => console.error("Error fetching todos:", err));
  }, []);

  // WRITE Record
  const addTodo = async (e) => {
    e.preventDefault();
    if (!newTodo.trim()) return;

    const response = await fetch(`${API_URL}/api/todos`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ title: newTodo, isCompleted: false })
    });

    if (response.ok) {
      const createdTodo = await response.json();
      setTodos([...todos, createdTodo]);
      setNewTodo('');
    }
  };

  return (
    <div style={{ padding: '20px', fontFamily: 'sans-serif' }}>
      <h1>Todo App (.NET 10 + React)</h1>
      <form onSubmit={addTodo}>
        <input 
          type="text" 
          value={newTodo} 
          onChange={(e) => setNewTodo(e.target.value)} 
          placeholder="Enter a task..." 
        />
        <button type="submit">Add Task</button>
      </form>
      <ul>
        {todos.map(todo => (
          <li key={todo.id}>{todo.title}</li>
        ))}
      </ul>
    </div>
  );
}

export default App;
