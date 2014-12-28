using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrentStackAndQueue
{
	class Program
	{
		private static Queue<ItemsCollection> _queueCollection;
		private static ConcurrentQueue<ItemsCollection> _concurrentQueue;
		private static Stack<ItemsCollection> _stackCollection;
		private static ConcurrentStack<ItemsCollection> _concurrentStack; 

		private static void CreateMenu()
		{
			Console.Clear();
			Console.WriteLine("Choose your test:");
			Console.WriteLine("1 - Stack collection");
			Console.WriteLine("2 - Concurrent stack collection");
			Console.WriteLine("1 - Queue collection");
			Console.WriteLine("2 - Concurrent queue collection");
			Console.WriteLine("Put a number and press the Enter");
		}

		private static void CreateObjects()
		{
			_stackCollection = new Stack<ItemsCollection>(PopulateItems());
			_concurrentStack = new ConcurrentStack<ItemsCollection>(PopulateItems());
			_queueCollection = new Queue<ItemsCollection>(PopulateItems());
			_concurrentQueue = new ConcurrentQueue<ItemsCollection>(PopulateItems());
		}

		static void Main(string[] args)
		{
			CreateMenu();
			CreateObjects();
			var number = Console.ReadLine();
			int nr = 0;
			if (int.TryParse(number, out nr))
			{
				switch (nr)
				{
					case 1:
						StackCollection();
						break;
					case 2:
						ConcurrentStackCollection();
						break;
					case 3:
						QueueCollection();
						break;
					case 4:
						ConcurrentQueueCollection();
						break;
				}
			}
			Console.ReadLine();
		}

		private static List<ItemsCollection> PopulateItems()
		{
			List<ItemsCollection> collection = new List<ItemsCollection>();
			for (int i = 0; i < 100000; i++)
			{
				collection.Add(new ItemsCollection
				{
					FirstName = "FirstName " + i,
					LastName = "LastName " + i
				});
			}
			return collection;
		}

		private static void QueueCollection()
		{
			Task.Factory.StartNew(() =>
								  {
									  while (true)
									  {
										  Task.Delay(200);
										  Console.WriteLine("First thread " + _queueCollection.Dequeue().FirstName);
										  Task.Delay(200);
									  }
								  });
			Task.Factory.StartNew(() =>
			                      {
									  while (true)
									  {
										  Task.Delay(200);
										  Console.WriteLine("Second thread " + _queueCollection.Dequeue().FirstName);
										  Task.Delay(200);
									  }
			                      });
		}

		private static void ConcurrentQueueCollection()
		{
			Task.Factory.StartNew(() =>
			                      {
									  while (true)
									  {
										  Task.Delay(200);
										  ItemsCollection item;
										  if (_concurrentQueue.TryDequeue(out item))
										  {
											  Console.WriteLine("First thread " + item.FirstName);
										  }
										  Task.Delay(200);
									  }
			                      });
			Task.Factory.StartNew(() =>
			                      {
									  while (true)
									  {
										  Task.Delay(200);
										  ItemsCollection item;
										  if (_concurrentQueue.TryDequeue(out item))
										  {
											  Console.WriteLine("Second thread " + item.FirstName);
										  }
										  Task.Delay(200);
									  }
			                      });
		}

		private static void StackCollection()
		{
			Task.Factory.StartNew(() =>
			{
				while (true)
				{
					Task.Delay(200);
					Console.WriteLine("First thread " + _stackCollection.Pop().FirstName);
					Task.Delay(200);
				}
			});
			Task.Factory.StartNew(() =>
			{
				while (true)
				{
					Task.Delay(200);
					Console.WriteLine("Second thread " + _stackCollection.Pop().FirstName);
					Task.Delay(200);
				}
			});
		}

		private static void ConcurrentStackCollection()
		{
			Task.Factory.StartNew(() =>
			{
				while (true)
				{
					Task.Delay(200);
					ItemsCollection item;
					if (_concurrentStack.TryPop(out item))
					{
						Console.WriteLine("First thread " + item.FirstName);
					}
					Task.Delay(200);
				}
			});
			Task.Factory.StartNew(() =>
			{
				while (true)
				{
					Task.Delay(200);
					ItemsCollection item;
					if (_concurrentStack.TryPop(out item))
					{
						Console.WriteLine("Second thread " + item.FirstName);
					}
					Task.Delay(200);
				}
			});
		}
	}
}
