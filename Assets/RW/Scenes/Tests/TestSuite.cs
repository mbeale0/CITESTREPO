using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class TestSuite
{
    private Game game;
    [SetUp]
    public void Setup()
    {
        GameObject gameGameObject =
            MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Game"));
        game = gameGameObject.GetComponent<Game>();
    }
    [TearDown]
    public void Teardown()
    {
        Object.Destroy(game.gameObject);
    }

    [UnityTest]
    public IEnumerator ShipMovesLeft()
    {
        GameObject ship = game.GetShip().gameObject;
        float initXPos = ship.transform.position.x;
        game.GetShip().MoveLeft();
        
        yield return new WaitForSeconds(0.1f);

        Assert.Less(ship.transform.position.x, initXPos);
    }
    [UnityTest]
    public IEnumerator ShipMovesRight()
    {
        GameObject ship = game.GetShip().gameObject;
        float initXPos = ship.transform.position.x;
        game.GetShip().MoveRight();

        yield return new WaitForSeconds(0.1f);

        Assert.Greater(ship.transform.position.x, initXPos);
    }
    [UnityTest]
    public IEnumerator AsteroidsMoveDown()
    {
        GameObject asteroid = game.GetSpawner().SpawnAsteroid();

        float initialYPos = asteroid.transform.position.y;
        yield return new WaitForSeconds(0.1f);

        Assert.Less(asteroid.transform.position.y, initialYPos);
    }
    [UnityTest]
    public IEnumerator GameOverOccursOnAsteroidCollision()
    {
        GameObject asteroid = game.GetSpawner().SpawnAsteroid();
        asteroid.transform.position = game.GetShip().transform.position;

        yield return new WaitForSeconds(0.1f);

        Assert.True(game.isGameOver);

    }
    [UnityTest]
    public IEnumerator NewGameRestartsGame()
    {
        //1
        game.isGameOver = true;
        game.NewGame();
        //2
        Assert.False(game.isGameOver);
        yield return null;
    }
    public IEnumerator NewGameResetsScore()
    {
        //makes sure score is not 0
        game.SetScore(20);
        game.NewGame();
        Assert.AreEqual(game.GetScore(), 0);
        yield return null;
    }
    [UnityTest]
    public IEnumerator LaserMovesUp()
    {
        // 1
        GameObject laser = game.GetShip().SpawnLaser();
        // 2
        float initialYPos = laser.transform.position.y;
        yield return new WaitForSeconds(0.1f);
        // 3
        Assert.Greater(laser.transform.position.y, initialYPos);
    }
    [UnityTest]
    public IEnumerator LaserDestroysAsteroid()
    {
        // 1
        GameObject asteroid = game.GetSpawner().SpawnAsteroid();
        asteroid.transform.position = Vector3.zero;
        GameObject laser = game.GetShip().SpawnLaser();
        laser.transform.position = Vector3.zero;
        yield return new WaitForSeconds(0.1f);
        // 2
        UnityEngine.Assertions.Assert.IsNull(asteroid);
    }
    [UnityTest]
    public IEnumerator DestroyAsteroidAddsScore()
    {
        int initScore = game.GetScore();
        GameObject asteroid = game.GetSpawner().SpawnAsteroid();
        asteroid.transform.position = Vector3.zero;
        GameObject laser = game.GetShip().SpawnLaser();
        laser.transform.position = Vector3.zero;
        
        yield return new WaitForSeconds(.1f);
        int newScore = game.GetScore();
        Assert.AreEqual(newScore, initScore + 1);
    }
}

