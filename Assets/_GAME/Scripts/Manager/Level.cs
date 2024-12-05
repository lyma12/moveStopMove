using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Level : GameUnit, IObserver
{
    [SerializeField]
    private int numberOfEnemy = 20;
    [SerializeField]
    private Bot bot;
    [SerializeField]
    private List<PantType> pantTypes = new List<PantType>();
    [SerializeField]
    private List<SkinType> skinTypes = new List<SkinType>();
    [SerializeField]
    private List<WeaponType> weaponTypes = new List<WeaponType>();
    private int currentEnemy = 0;
    private List<Character> listCurrentEnemy = new List<Character>();
    private int currentActiveEnemy = 0;
    private IEnumerator SpawnEnemy()
    {
        Vector3 playerTransform = LevelManager.Instance.Player.transform.position;
        Vector2 randomPos2D = Random.insideUnitCircle * Contanst.DistanceMaxSpawnBot;
        Vector3 randomPos = new Vector3(randomPos2D.x, playerTransform.y, randomPos2D.y) + playerTransform;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPos, out hit, Contanst.DistanceMaxSpawnBot, NavMesh.AllAreas))
        {
            Bot botSpawn = SimplePool.Spawn<Bot>(PoolType.Bot, randomPos, Quaternion.identity);
            // var spawnObject = ObjectPoolManager.Instance.SpawnObject(bot.gameObject, randomPos, Quaternion.identity, transform);
            //Bot b = Cache.Instance.GetCharacter<Bot>(botSpawn.transform);
            botSpawn.Attach(this);
            ChangeEquiment(botSpawn);
            currentActiveEnemy++;
            currentEnemy++;
            listCurrentEnemy.Add(botSpawn);
        }
        yield return null;
    }
    private void FixedUpdate()
    {
        if (LevelManager.Instance.LevelState == LevelState.PLAY)
        {
            if (currentActiveEnemy < 5 && currentEnemy < numberOfEnemy)
            {
                StartCoroutine(SpawnEnemy());
            }
        }
    }
    private void OnBotDie()
    {
        currentActiveEnemy--;
        if (currentEnemy >= numberOfEnemy && currentActiveEnemy <= 0)
        {
            LevelManager.Instance.LevelState = LevelState.ON_VICTORY;
        }
    }
    private void ChangeEquiment(Character character)
    {
        character.ChangeWeapon(RandomUIType<WeaponType>(weaponTypes));
        character.ChangePant(RandomUIType<PantType>(pantTypes));
        character.ChangeSkin(RandomUIType<SkinType>(skinTypes));
    }
    private T RandomUIType<T>(List<T> listType)
    {
        int random = Random.Range(0, listType.Count);
        return listType[random];
    }
    public void Reset()
    {
        currentActiveEnemy = 0;
        currentEnemy = 0;
        foreach (var i in listCurrentEnemy)
        {
            if (i != null)
            {
                SimplePool.Despawn(i);
            }
        }
        listCurrentEnemy.Clear();
    }

    public void UpdateState(ISubject subject)
    {
        if (subject is Bot)
        {
            OnBotDie();
        }
    }

    public override void OnInit()
    {

    }

    public override void OnDespawn()
    {
        Reset();
    }
}