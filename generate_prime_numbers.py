# https://www.geeksforgeeks.org/sieve-eratosthenes-0n-time-complexity/
def getSieve(n):
    """Returns array with prime numbers up to n.
    Computes such array in O(n) time/space using Sieve of Eratosthenes"""

    isprime = [True for _ in range(n)]  
    prime = []  
    spf = [None for _ in range(n)]
        
    isprime[0] = isprime[1] = False
    for i in range(2, n):
        if isprime[i]:
            prime.append(i)
            spf[i] = i
                
        j = 0
        while (j < len(prime) and i * prime[j] < n and prime[j] <= spf[i]):
            isprime[i * prime[j]] = False
            spf[i * prime[j]] = prime[j] 
                
            j += 1

    return prime

def is_probably_prime(n, sieve):
    for x in sieve:
        if n % x == 0:
            return False
    return True

def gcd(a, b):
    """Euclid's algorithm for GCD
    Given input a, b the function returns d such that gcd(a,b) = d"""

    if a < b:
        a,b = b,a

    while b != 0:
        a, b = b, a % b
    return a

def modpow(x, y, p):
    x %= p
    if x == 0: return 0

    res = 1 
    while y > 0:
        if y & 1: res = (res * x) % p

        y >>= 1
        x = (x * x) % p         
    return res

import random, math
def generatePrime(n : int, primes = None, s = None):
    """Generates prime number with at least n digits:

    : param n: number of 10-based digits in the generate prime is at least n;
    : param primes: iterable object of numbers that are used as small factors
    for pre-prime verification. If None, is computed using getSieve(1000);
    : param s: initial prime number - if None, last from primes is used;
    """

    # Any prime number higher than the up_limit suffices the result.
    up_limit = 10**(n-1)

    # Get the list of small primes which are used as small divisors
    # to reject the n candidate before the Pocklington test.
    if not primes: primes = getSieve(100)

    if not s: s = primes[-1] # initial prime
    while s < up_limit:
        lo, hi = (s + 1) >> 1, 2*s + 1

        # Proceed with finding new prime n
        while True:
            r = random.randint(lo, hi) << 1 # get random even r from [s, 4*s + 2]
            n = s*r + 1 # n is prime candidate, s^2 + 1 <= n <= 4s^2 + 2s + 1

            # reject n if n divides some number in primes list
            if not is_probably_prime(n, primes): continue

            # Here n is probably prime - apply Pocklington criterion to verify it
            while True:
                a = random.randint(2, n-1)

                # Fermat’s little theorem isn't satisfied - choose another n
                if pow(a, n-1, n) != 1: 
                    break

                d = math.gcd((pow(a, r, n) - 1) % n, n)
                if d != n:
                    if d == 1: s = n # n is prime, replace s with n
                    else: pass # n isn't prime, choose another n
                    break
                else: pass # a^r mod n == 1, choose another a
            if s == n: break
    return s

import sympy
def test(n, k):
    primes = getSieve(1000)
    for x in range(n >> 1, n):
        print(x)
        for _ in range(k):
            assert sympy.ntheory.primetest.isprime(generatePrime(x, primes))


def encript(m, e, n):
    return pow(m, e, n)

def decript(c, d, n):
    return pow(c, d, n)

from numba import njit, prange, parallel_chunksize, cuda
import numpy as np
import time
from sympy import nextprime

@njit(parallel=True)
def get_d_key(e_key, fn_n, max_len):
    d_min_val = e_key ** -1.0 % fn_n
    for i in prange(10**(max_len+2)):
        value = d_min_val * fn_n * i + d_min_val
        if value * e_key % fn_n != 1:
            continue
        if int(value) - value != 0:
            continue
        return int(value)
    raise Exception()

@cuda.jit#('void(int64[:], int64[:], float64[:], int64[:], int64[:])')
def cuda_d_key(result, cuda_i_start, cuda_n, d_min_val, e_key, fn_n):
    i = cuda.blockIdx.x * cuda.blockDim.x + cuda.threadIdx.x
    #if i >= cuda_n[0]:
    #    return
    cuda_i_start = cuda_i_start[0]
    d_min_val = float(d_min_val[0])
    e_key = e_key[0]
    fn_n = fn_n[0]
    value = d_min_val * fn_n * (cuda_i_start + cuda_n[0] - i) + d_min_val
    if value * e_key % fn_n != 1:
        return
    if int(value) - value != 0:
        return
    value = int(value)
    cuda.syncthreads()
    if result[0] < value:
        print(value)
        result[0] = value

#@njit
def extended_gcd_requs(a, b):
    if a == 0:
        return (b, 0, 1)
    gcd, x1, y1 = extended_gcd(b % a, a)
    x = y1 - (b // a) * x1
    y = x1
    return (gcd, x, y)
    
#@njit
def extended_gcd_cykle(a, b):
    r0, r1 = a, b
    s0, s1 = 1, 0
    t0, t1 = 0, 1

    while r1 != 0:
        q = r0 // r1
        r0, r1 = r1, r0 - q * r1
        s0, s1 = s1, s0 - q * s1
        t0, t1 = t1, t0 - q * t1
    return r0, s0, t0

def mod_inverse(e, phi):
    gcd, x, _ = extended_gcd_cykle(e, phi)
    if gcd != 1:
        raise Exception("Обратного элемента не существует!")
    return x % phi

if __name__ == '__main__':
    #test(20, 10000)
    random.seed(42)
    size = 300
    #p_prime = generatePrime(size)
    #p_prime = 401
    p_prime = nextprime(2**1024)
    p_str = str(p_prime)

    #q_prime = generatePrime(size)
    #while q_prime in [p_prime]:
    #    q_prime = generatePrime(size, [p_prime])
    #q_prime = 443
    q_prime = nextprime(2**2048 // p_prime)
    while q_prime in [p_prime]:
        q_prime = nextprime(q_prime)
    q_str = str(q_prime)

    #e_key = generatePrime(size)
    #while e_key in [p_prime, q_prime]:
    #    e_key = generatePrime(size)
    #e_key = 101
    #e_key = 65537
    e_key = nextprime(2**1024)
    while e_key in [p_prime, q_prime]:
        e_key = nextprime(e_key)
    e_str = str(e_key)

    n_key = p_prime * q_prime
    n_str = str(n_key)

    fn_n = (p_prime - 1) * (q_prime - 1)
    fn_n_str = str(fn_n)

    print('p', p_str, len(p_str))
    print('q', q_str, len(q_str))
    print('e', e_str, len(e_str))
    print('n', n_str, len(n_str))
    print('fn_n', fn_n_str, len(fn_n_str))
    

    # CUDA
#    d_min_val = e_key ** -1.0 % fn_n
#    d_max_size = 10 ** (len(fn_n_str) + 2)
#    print(d_max_size)
#    i_start = 0
#    i_end = None
#    step = 10**10
#    d_key = None
#    while True:
#        i_end = i_start + step
#        if i_end > d_max_size:
#            i_end = d_max_size
#        cuda_n = i_end - i_start
#        result = np.array([-1])
#
#        device = cuda.get_current_device()
#        cuda_result = cuda.to_device(result)
#        cuda_i_start = cuda.to_device([i_start])
#        cuda_n_value = cuda.to_device([cuda_n])
#        cuda_d_min_val = cuda.to_device([d_min_val])
#        cuda_e_key = cuda.to_device([e_key])
#        cuda_fn_n = cuda.to_device([fn_n])
#
#        tpb = device.WARP_SIZE
#        bpg = int(np.ceil(cuda_n / tpb))
#        print('start', i_start, i_end, cuda_n)
#        cuda_d_key[bpg, tpb](cuda_result, cuda_i_start, cuda_n_value, cuda_d_min_val, cuda_e_key, cuda_fn_n)
#        result = cuda_result.copy_to_host()[0]
#        print('finish', result)
#        if result != -1:
#            d_key = int(result)
#            break
#        i_start = i_end
#        if i_end >= d_max_size:
#            break
#        break
#
#    d_str = str(d_key)
#    print('d', d_str, len(d_str))

    ## Numba
    #d_key = get_d_key(e_key, fn_n, len(fn_n_str))
    #d_str = str(d_key)
    #print('d', d_str, len(d_str))

    # GCD
    
    d_key = mod_inverse(e_key, fn_n)
    d_str = str(d_key)
    print('d', d_str, len(d_str))
    m = 123
    c = encript(m, e_key, n_key)
    c_str = str(c)
    m = decript(c, d_key, n_key)
    m_str = str(m)

    print(c_str, len(c_str))
    print(m_str, len(m_str))
    assert sympy.ntheory.primetest.isprime(p_prime)
    assert sympy.ntheory.primetest.isprime(q_prime)
    assert sympy.ntheory.primetest.isprime(e_key)
    print(nextprime(c))