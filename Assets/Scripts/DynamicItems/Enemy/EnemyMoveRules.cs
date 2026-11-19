using System;
using System.Collections.Generic;
using UnityEngine;

namespace DynamicItems.Enemy
{
    public sealed class EnemyMoveRules
    {
        private enum Edge
        {
            Bottom,
            Top,
            Left,
            Right
        }

        private readonly Rect _bounds;
        private readonly System.Random _random;
        private const float Epsilon = 0.1f;

        public EnemyMoveRules(Rect bounds, System.Random random = null)
        {
            if (bounds.width <= 0f || bounds.height <= 0f)
            {
                throw new ArgumentException(
                    "長方形の幅と高さは0より大きい必要があります。");
            }

            _bounds = bounds;
            _random = random ?? new System.Random();
        }

        /// <summary>
        /// 開始点から辺上を移動し、停止点の配列を返す。
        /// 停止点の数はランダムに1～3個。
        /// </summary>
        public Vector2[] Generate(Vector2 start)
        {
            ValidateStart(start);

            // 移動回数を1～3で決定
            int moveCount = _random.Next(1, 4);

            Edge currentEdge = GetStartingEdge(start);

            var points = new List<Vector2>(moveCount);

            // 1回なら、現在いる辺上のランダムな点で止まる
            if (moveCount == 1)
            {
                points.Add(RandomPointOnEdge(currentEdge));
                return points.ToArray();
            }

            // 最初の移動：現在の辺の端まで進む
            Vector2 corner;

            if (IsCorner(start))
            {
                // すでに角にいる場合は、選択した辺に沿って進む
                corner = OtherCornerOnEdge(currentEdge, start);
            }
            else
            {
                // 辺の途中なら、左右または上下の端をランダムに選ぶ
                corner = RandomEndpoint(currentEdge);
            }

            points.Add(corner);

            Edge previousEdge = currentEdge;

            // 中間の移動：来た辺を戻らず、次の角まで進む
            for (int i = 1; i < moveCount - 1; i++)
            {
                Edge nextEdge = OtherEdgeAtCorner(
                    corner,
                    previousEdge);

                corner = OtherCornerOnEdge(nextEdge, corner);

                points.Add(corner);
                previousEdge = nextEdge;
            }

            // 最後の移動：来た辺とは別の辺上のランダムな点で止まる
            Edge finalEdge = OtherEdgeAtCorner(
                corner,
                previousEdge);

            points.Add(RandomPointOnEdge(finalEdge));

            return points.ToArray();
        }

        private Edge GetStartingEdge(Vector2 start)
        {
            bool left = IsNear(start.x, _bounds.xMin);
            bool right = IsNear(start.x, _bounds.xMax);
            bool bottom = IsNear(start.y, _bounds.yMin);
            bool top = IsNear(start.y, _bounds.yMax);

            // 角では、接続している2辺のどちらかをランダムに選ぶ
            if ((left || right) && (bottom || top))
            {
                Edge vertical = left ? Edge.Left : Edge.Right;
                Edge horizontal = bottom ? Edge.Bottom : Edge.Top;

                return _random.Next(0, 2) == 0
                    ? vertical
                    : horizontal;
            }

            if (left) return Edge.Left;
            if (right) return Edge.Right;
            if (bottom) return Edge.Bottom;

            return Edge.Top;
        }

        private Vector2 RandomPointOnEdge(Edge edge)
        {
            float t = (float)_random.NextDouble();

            switch (edge)
            {
                case Edge.Bottom:
                    return new Vector2(
                        Lerp(_bounds.xMin, _bounds.xMax, t),
                        _bounds.yMin);

                case Edge.Top:
                    return new Vector2(
                        Lerp(_bounds.xMin, _bounds.xMax, t),
                        _bounds.yMax);

                case Edge.Left:
                    return new Vector2(
                        _bounds.xMin,
                        Lerp(_bounds.yMin, _bounds.yMax, t));

                case Edge.Right:
                    return new Vector2(
                        _bounds.xMax,
                        Lerp(_bounds.yMin, _bounds.yMax, t));

                default:
                    throw new ArgumentOutOfRangeException(nameof(edge));
            }
        }

        private Vector2 RandomEndpoint(Edge edge)
        {
            Vector2 a;
            Vector2 b;

            switch (edge)
            {
                case Edge.Bottom:
                    a = new Vector2(_bounds.xMin, _bounds.yMin);
                    b = new Vector2(_bounds.xMax, _bounds.yMin);
                    break;

                case Edge.Top:
                    a = new Vector2(_bounds.xMin, _bounds.yMax);
                    b = new Vector2(_bounds.xMax, _bounds.yMax);
                    break;

                case Edge.Left:
                    a = new Vector2(_bounds.xMin, _bounds.yMin);
                    b = new Vector2(_bounds.xMin, _bounds.yMax);
                    break;

                case Edge.Right:
                    a = new Vector2(_bounds.xMax, _bounds.yMin);
                    b = new Vector2(_bounds.xMax, _bounds.yMax);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(edge));
            }

            return _random.Next(0, 2) == 0 ? a : b;
        }

        // 角で、直前に進んできた辺とは別の辺を取得する
        private Edge OtherEdgeAtCorner(
            Vector2 corner,
            Edge previousEdge)
        {
            bool left = IsNear(corner.x, _bounds.xMin);
            bool bottom = IsNear(corner.y, _bounds.yMin);

            Edge vertical = left ? Edge.Left : Edge.Right;
            Edge horizontal = bottom ? Edge.Bottom : Edge.Top;

            if (previousEdge == vertical)
                return horizontal;

            if (previousEdge == horizontal)
                return vertical;

            throw new InvalidOperationException(
                "指定された辺は、この角に接続していません。");
        }

        // 指定された辺に沿って、現在の角とは反対側の角を取得する
        private Vector2 OtherCornerOnEdge(
            Edge edge,
            Vector2 corner)
        {
            switch (edge)
            {
                case Edge.Bottom:
                    return new Vector2(
                        IsNear(corner.x, _bounds.xMin)
                            ? _bounds.xMax : _bounds.xMin,
                        _bounds.yMin);

                case Edge.Top:
                    return new Vector2(
                        IsNear(corner.x, _bounds.xMin)
                            ? _bounds.xMax : _bounds.xMin,
                        _bounds.yMax);

                case Edge.Left:
                    return new Vector2(
                        _bounds.xMin,
                        IsNear(corner.y, _bounds.yMin)
                            ? _bounds.yMax : _bounds.yMin);

                case Edge.Right:
                    return new Vector2(
                        _bounds.xMax,
                        IsNear(corner.y, _bounds.yMin)
                            ? _bounds.yMax : _bounds.yMin);

                default:
                    throw new ArgumentOutOfRangeException(nameof(edge));
            }
        }

        private bool IsCorner(Vector2 point)
        {
            bool leftOrRight =
                IsNear(point.x, _bounds.xMin) ||
                IsNear(point.x, _bounds.xMax);

            bool topOrBottom =
                IsNear(point.y, _bounds.yMin) ||
                IsNear(point.y, _bounds.yMax);

            return leftOrRight && topOrBottom;
        }

        private void ValidateStart(Vector2 start)
        {
            bool inside =
                start.x >= _bounds.xMin - Epsilon &&
                start.x <= _bounds.xMax + Epsilon &&
                start.y >= _bounds.yMin - Epsilon &&
                start.y <= _bounds.yMax + Epsilon;

            bool onBoundary =
                IsNear(start.x, _bounds.xMin) ||
                IsNear(start.x, _bounds.xMax) ||
                IsNear(start.y, _bounds.yMin) ||
                IsNear(start.y, _bounds.yMax);

            if (!inside || !onBoundary)
            {
                throw new ArgumentException(
                    "開始点は長方形の辺上に指定してください。",
                    nameof(start));
            }
        }

        private static float Lerp(float min, float max, float t)
        {
            return min + (max - min) * t;
        }

        private static bool IsNear(float a, float b)
        {
            return Math.Abs(a - b) <= Epsilon;
        }
    }
}