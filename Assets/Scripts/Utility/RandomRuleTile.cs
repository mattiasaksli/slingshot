using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New Random Rule Tile", menuName = "Tiles/Random Rule Tile")]
public class RandomRuleTile : RuleTile
{
    protected new static float GetPerlinValue(Vector3Int position, float scale, float offset)
    {
        return UnityEngine.Random.value;
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        var iden = Matrix4x4.identity;

        tileData.sprite = m_DefaultSprite;
        tileData.gameObject = m_DefaultGameObject;
        tileData.colliderType = m_DefaultColliderType;
        tileData.flags = TileFlags.LockTransform;
        tileData.transform = iden;

        foreach (TilingRule rule in m_TilingRules)
        {
            Matrix4x4 transform = iden;
            if (RuleMatches(rule, position, tilemap, ref transform))
            {
                switch (rule.m_Output)
                {
                    case TilingRule.OutputSprite.Single:
                    case TilingRule.OutputSprite.Animation:
                        tileData.sprite = rule.m_Sprites[0];
                        break;
                    case TilingRule.OutputSprite.Random:
                        int index = Mathf.Clamp(Mathf.FloorToInt(GetPerlinValue(position, rule.m_PerlinScale, 100000f) * rule.m_Sprites.Length), 0, rule.m_Sprites.Length - 1);
                        tileData.sprite = rule.m_Sprites[index];
                        if (rule.m_RandomTransform != TilingRule.Transform.Fixed)
                            transform = ApplyRandomTransform(rule.m_RandomTransform, transform, rule.m_PerlinScale, position);
                        break;
                }
                tileData.transform = transform;
                tileData.gameObject = rule.m_GameObject;
                tileData.colliderType = rule.m_ColliderType;
                break;
            }
        }
    }
}