public struct CoinPile
{
    private uint _ones, _twos, _fives, _tens;

    public CoinPile(uint ones = 0, uint twos = 0, uint fives = 0, uint tens = 0)
    {
        _ones = ones;
        _twos = twos;
        _fives = fives;
        _tens = tens;
    }

    public CoinPile(CoinPile coins)
    {
        _ones = coins._ones;
        _twos = coins._twos;
        _fives = coins._fives;
        _tens = coins._tens;
    }

    public uint Total()
    {
        return _ones + _twos * 2 + _fives * 5 + _tens * 10;
    }

    public void Add(CoinPile coins)
    {
        _ones += coins._ones;
        _twos += coins._twos;
        _fives += coins._fives;
        _tens += coins._tens;
    }

    public CoinPile Subtract(CoinPile coins)
    {
        if (Total() < coins.Total())
            throw new TransactionException("Недостаточно денег.");

        if (_ones < coins._ones || _twos < coins._twos || _fives < coins._fives || _tens < coins._tens)
            throw new TransactionException("Не получится извлечь данные монеты.");

        _ones -= coins._ones;
        _twos -= coins._twos;
        _fives -= coins._fives;
        _tens -= coins._tens;

        return coins;
    }

    public CoinPile Subtract(uint sum)
    {
        if (Total() < sum)
            throw new TransactionException("Недостаточно денег.");

        uint pendingSum = sum;

        uint pendingTens = Math.Min(pendingSum / 10, _tens);
        pendingSum -= pendingTens * 10;

        uint pendingFives = Math.Min(pendingSum / 5, _fives);
        pendingSum -= pendingFives * 5;

        uint pendingTwos = Math.Min(pendingSum / 2, _twos);
        pendingSum -= pendingTwos * 2;

        if (pendingSum > _ones)
        {
            throw new TransactionException("Невозможно подобрать сумму для размена.");
        }
        else
        {
            _tens -= pendingTens;
            _fives -= pendingFives;
            _twos -= pendingTwos;
            _ones -= pendingSum;
        }

        return new CoinPile(pendingSum, pendingTwos, pendingFives, pendingTens);
    }

    public override string ToString()
    {
        return (Total() == 0 ? "0 кредитов." : $"{Total()} кредитов:\n")
            + (_ones > 0 ? $"{_ones} 1-кредитных монет\n" : "")
            + (_twos > 0 ? $"{_twos} 2-кредитных монет\n" : "")
            + (_fives > 0 ? $"{_fives} 5-кредитных монет\n" : "")
            + (_tens > 0 ? $"{_tens} 10-кредитных монет" : "");
    }
}
