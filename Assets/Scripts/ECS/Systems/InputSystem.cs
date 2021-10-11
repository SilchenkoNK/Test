using UnityEngine;
using Leopotam.Ecs;

public class InputSystem : IEcsRunSystem
{
    private EcsFilter<InputComponent> _filter;

    bool _pressed = false;
    float _lastX;

    public void Run()
    {
        foreach (var i in _filter)
        {
            if (_pressed)
            {
                ref var input = ref _filter.Get1(i);

                if (Input.GetMouseButtonUp(0))
                {
                    _pressed = false;
                    input.DeltaX = 0f;
                }
                else
                    input.DeltaX = Input.mousePosition.x - _lastX;
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                    _pressed = true;
            }

            _lastX = Input.mousePosition.x;
        }
    }
}
